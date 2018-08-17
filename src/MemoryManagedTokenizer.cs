using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class MemoryManagedTokenizer : MultiThreadedStreamTokenizer
    {
        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private readonly ConcurrentQueue<ISection> _queue = new ConcurrentQueue<ISection>();
        private int _memoryPoolRentSize;

        public MemoryManagedTokenizer(Stream stream)
            : this(stream, null)
        {
        }

        public MemoryManagedTokenizer(Stream stream, TokenizerSettings settings)
            : base(stream, settings)
        {
        }

        protected override Task<ISection> GetNextAsyncInternal()
        {

            do
            {
                if (_queue.TryDequeue(out ISection result))
                {
                    return Task.FromResult(result);
                }
            }
            while (!ReadFinished);


            Completed = true;
            return Task.FromResult<ISection>(null);
        }

        protected override Task Read(CancellationToken token)
        {
            _memoryPoolRentSize = Math.Min(_memoryPool.MaxBufferSize, (int)Stream.Length);

            var memoryOwner = _memoryPool.Rent(_memoryPoolRentSize);
            int bytesRead = 0;
            bool consumeSectionDelimiters = false;
            var segmentHints = new List<int>();

            int memoryStartIndex = 0;
            int rentedChunks = 1;

            while (!Disposed)
            {
                // Read to the end of the line in the stream
                int b = Stream.ReadByte();

                if (b == -1)
                {
                    break;
                }

                if (consumeSectionDelimiters)
                {
                    while (IsSectionDelimiter((byte)b))
                    {
                        b = Stream.ReadByte();

                        if (b == -1)
                        {
                            break;
                        }
                    }

                    consumeSectionDelimiters = false;
                }

                var memoryIndex = memoryStartIndex + bytesRead;
                memoryOwner.Memory.Span[memoryIndex] = (byte)b;

                if (IsSectionDelimiter((byte)b))
                {
                    // Current memory represents a row. Write it to be read by the reader.
                    // Memory should be trimmed to only number of bytes read. It may have 
                    // been overallocated
                    QueueSection();
                    consumeSectionDelimiters = true;
                }
                else if (++bytesRead >= memoryOwner.Memory.Length)
                {
                    // The current allocated memory has run out of space, need to relocate
                    int rentSize = (int)Stream.Length - (rentedChunks * _memoryPoolRentSize);
                    var tmpMemory = _memoryPool.Rent(Math.Min(rentSize, _memoryPoolRentSize));

                    // Only copy memory that's been used
                    var memToCopy = memoryOwner.Memory.Slice(memoryStartIndex, bytesRead);
                    memToCopy.CopyTo(tmpMemory.Memory);

                    // Dispose of old memory and save new memory
                    memoryOwner.Dispose();
                    memoryOwner = tmpMemory;

                    // Reset the start index, since old memory is no longer managed here
                    memoryStartIndex = 0;
                }

                if ((bytesRead > 0) && IsSegmentDelimiter((byte)b))
                {
                    segmentHints.Add(bytesRead - 1);
                }
            }

            if (bytesRead != 0)
            {
                QueueSection();
            }

            void QueueSection()
            {
                var section = new MemoryManagedSection(memoryOwner, memoryStartIndex, bytesRead, segmentHints, Settings.Encoding);
                _queue.Enqueue(section);
                segmentHints.Clear();
                memoryStartIndex += bytesRead;
                bytesRead = 0;
            }

            return Task.CompletedTask;
        }

        private class MemoryManagedSection : MemoryOwningSection
        {
            private readonly Memory<byte> _memory;
            private readonly Encoding _encoding;

            public MemoryManagedSection(IMemoryOwner<byte> owner, int start, int size, List<int> segmentHints, Encoding encoding)
                : base(owner, segmentHints, encoding)
            {
                _memory = owner.Memory.Slice(start, size);
                _encoding = encoding;
            }

            protected override Memory<byte> Memory => _memory;
        }
    }
}

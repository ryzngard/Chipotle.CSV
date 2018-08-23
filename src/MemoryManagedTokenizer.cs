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

        protected override async Task Read(CancellationToken token)
        {
            _memoryPoolRentSize = Math.Min(_memoryPool.MaxBufferSize, (int)Stream.Length);

            var memoryOwner = _memoryPool.Rent(_memoryPoolRentSize);
            var segmentHints = new List<int>();
            int rentedChunks = 1;
            int consumedMemoryIndex = 0;

            while (!Disposed)
            {
                int memoryStartIndex = 0;
                int bytesRead = await Stream.ReadAsync(memoryOwner.Memory.Slice(consumedMemoryIndex));
                bool consumeSectionDelimiters = false;

                if (bytesRead == 0)
                {
                    break;
                }

                for (int i = 0; i < bytesRead; i++)
                {
                    // Read to the end of the line in the stream
                    var b = memoryOwner.Memory.Span[i];

                    if (consumeSectionDelimiters)
                    {
                        if (IsSectionDelimiter(b))
                        {
                            continue;
                        }

                        consumeSectionDelimiters = false;
                    }

                    // The relative index into the memory, 0 indexed
                    var memoryIndex = i - memoryStartIndex;

                    if (IsSectionDelimiter(b))
                    {
                        // Current memory represents a row. Write it to be read by the reader.
                        // Memory should be trimmed to only number of bytes read. It may have 
                        // been overallocated
                        QueueSection(memoryStartIndex, memoryIndex+1);
                        consumeSectionDelimiters = true;
                        memoryStartIndex = i;
                    }

                    if (IsSegmentDelimiter(b))
                    {
                        segmentHints.Add(memoryIndex);
                    }
                }

                if (memoryStartIndex == 0)
                {
                    // None of the memory was used, check if the whole stream has been read
                    if (bytesRead == Stream.Length)
                    {
                        // The whole memory chunk is a section
                        QueueSection(0, bytesRead);
                    }
                    else
                    {
                        throw new OutOfMemoryException($"Unable to allocate enough memory to read section of stream. May be improved in the future");
                    }
                }
                else if ((memoryStartIndex+1) < bytesRead)
                {
                    // The current allocated memory has run out of space, need to relocate
                    int rentSize = (int)Stream.Length - (rentedChunks * _memoryPoolRentSize);
                    var tmpMemory = _memoryPool.Rent(Math.Min(rentSize, _memoryPoolRentSize));

                    // Only copy memory that's been used
                    var memToCopy = memoryOwner.Memory.Slice(memoryStartIndex, bytesRead-memoryStartIndex);
                    memToCopy.CopyTo(tmpMemory.Memory);

                    // Dispose of old memory and save new memory
                    memoryOwner.Dispose();
                    memoryOwner = tmpMemory;

                    // Set the consumed memory index to accomodate the new memory that was copied
                    consumedMemoryIndex = (bytesRead - memoryStartIndex - 1);
                }

            }

            void QueueSection(int start, int size)
            {
                var section = new MemoryManagedSection(memoryOwner, start, size, segmentHints, Settings.Encoding);
                _queue.Enqueue(section);
                segmentHints.Clear();
            }
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

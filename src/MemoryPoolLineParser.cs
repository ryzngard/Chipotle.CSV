using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chipotle.CSV
{
    internal class MemoryPoolLineParser : IDisposable
    {
        private readonly ConcurrentQueue<IRow<byte>> _queue;
        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private readonly byte _seperator;
        private readonly Stream _stream;
        private readonly MemoryPoolRowProvider.RowParseMechanism _parseMechanism;
        private bool _disposed = false;

        public MemoryPoolLineParser(Stream stream, ConcurrentQueue<IRow<byte>> queue, byte seperator, MemoryPoolRowProvider.RowParseMechanism mechanism)
        {
            _stream = stream;
            _queue = queue;
            _seperator = seperator;
            _parseMechanism = mechanism;
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public void Read()
        {
            var memoryOwner = _memoryPool.Rent();
            int bytesRead = 0;
            bool consumeNewLines = false;
            int predictedLineSize = memoryOwner.Memory.Length;

            while (!_disposed)
            {
                // Read to the end of the line in the stream
                int b = _stream.ReadByte();

                if (b == -1)
                {
                    break;
                }

                if (consumeNewLines)
                {
                    while (Helpers.ByteIsNewLine((byte)b))
                    {
                        b = _stream.ReadByte();

                        if (b == -1)
                        {
                            break;
                        }
                    }

                    consumeNewLines = false;
                }

                memoryOwner.Memory.Span[bytesRead] = (byte)b;

                if (Helpers.ByteIsNewLine((byte)b))
                {
                    // Update current memory allocation prediction size
                    predictedLineSize = Math.Max(bytesRead, predictedLineSize);

                    // Current memory represents a row. Write it to be read by the reader.
                    // Memory should be trimmed to only number of bytes read. It may have 
                    // been overallocated
                    var row = CreateRow(memoryOwner, bytesRead);
                    _queue.Enqueue(row);
                    bytesRead = 0;
                    memoryOwner = _memoryPool.Rent(predictedLineSize);
                    consumeNewLines = true;

                }
                else if (++bytesRead >= memoryOwner.Memory.Length)
                {
                    // The current allocated memory has run out of space, need to relocate
                    var tmpMemory = _memoryPool.Rent(memoryOwner.Memory.Length * 2);
                    memoryOwner.Memory.CopyTo(tmpMemory.Memory);

                    memoryOwner.Dispose();

                    tmpMemory = memoryOwner;
                }
            }

            if (bytesRead != 0)
            {
                var row = new MemoryOwningRow<byte>(memoryOwner, bytesRead, (byte)_seperator);
                _queue.Enqueue(row);
            }
        }

        private IRow<byte> CreateRow(IMemoryOwner<byte> memoryOwner, int bytesRead)
        {
            return Helpers.CreateRow(_parseMechanism, memoryOwner, bytesRead, _seperator);
        }
    }
}

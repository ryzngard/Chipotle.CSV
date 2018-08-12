using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class MemoryPoolRowProvider : IRowProvider
    {
        public bool Completed { get; private set; }

        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private bool _disposed = false;
        private bool _hasRead = false;
        private bool _finishedReading = false;

        private readonly Stream _stream;
        private readonly char _seperator;

        private ConcurrentQueue<MemoryOwningRow<byte>> _queue = new ConcurrentQueue<MemoryOwningRow<byte>>();

        public MemoryPoolRowProvider(Stream stream, char seperator = ',')
        {
            _stream = stream;
            _seperator = seperator;

            _hasRead = true;
            Task.Run(() => ReadStream());
        }

        public void Dispose()
        {
            _disposed = true;
            _memoryPool.Dispose();
        }

        public async Task<Row<byte>> GetNextAsync()
        {
            if (Completed)
            {
                return null;
            }

            if (_disposed)
            {
                throw new InvalidOperationException("Object already disposed");
            }

            do
            {
                if (_queue.TryDequeue(out MemoryOwningRow<byte> result))
                {
                    return result;
                }
            }
            while (!_finishedReading);


            Completed = true;
            return null;
        }

        private void ReadStream()
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
                    while (ByteIsNewLine((byte)b))
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

                if (ByteIsNewLine((byte)b))
                {
                    // Update current memory allocation prediction size
                    predictedLineSize = Math.Max(bytesRead, predictedLineSize);

                    // Current memory represents a row. Write it to be read by the reader.
                    // Memory should be trimmed to only number of bytes read. It may have 
                    // been overallocated
                    var row = new MemoryOwningRow<byte>(memoryOwner, bytesRead, (byte)_seperator);
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

            _finishedReading = true;
        }

        private static bool ByteIsNewLine(byte b)
        {
            return b == (byte)'\n' || b == (byte)'\r';
        }
    }
}

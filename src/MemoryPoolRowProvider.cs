using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class MemoryPoolRowProvider : IRowProvider
    {
        public enum RowParseMechanism
        {
            Upfront,
            Streamed
        }

        public struct Configuration
        {
            public RowParseMechanism RowParseMechanism { get; set; }
        }

        private static Configuration DefaultConfiguration = new Configuration()
        {
            RowParseMechanism = RowParseMechanism.Upfront
        };

        public bool Completed { get; private set; }

        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private bool _disposed = false;
        private bool _hasRead = false;
        private bool _finishedReading = false;

        private Configuration _config;

        private readonly Stream _stream;
        private readonly char _seperator;

        private long _totalReadStarveTime = 0;

        private ConcurrentQueue<IRow<byte>> _queue = new ConcurrentQueue<IRow<byte>>();

        public MemoryPoolRowProvider(Stream stream, char seperator = ',', Configuration? config = null)
        {
            _stream = stream;
            _seperator = seperator;

            if (config == null)
            {
                config = DefaultConfiguration;
            }

            _config = config.Value;

            _hasRead = true;
            Task.Run(() => ReadStream());
        }

        public void Dispose()
        {
            _disposed = true;
            _memoryPool.Dispose();
        }

        public Task<IRow<byte>> GetNextAsync()
        {
            if (_disposed)
            {
                throw new InvalidOperationException("Object already disposed");
            }

            if (Completed)
            {
                return Task.FromResult<IRow<byte>>(null);
            }

            do
            {
                if (_queue.TryDequeue(out IRow<byte> result))
                {
                    return Task.FromResult(result);
                }
            }
            while (!_finishedReading);


            Completed = true;
            return Task.FromResult<IRow<byte>>(null);
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

            _finishedReading = true;
        }

        private IRow<byte> CreateRow(IMemoryOwner<byte> memoryOwner, int bytesRead)
        {
            switch(_config.RowParseMechanism)
            {
                case RowParseMechanism.Upfront:
                    return new MemoryOwningRow<byte>(memoryOwner, bytesRead, (byte)_seperator);

                case RowParseMechanism.Streamed:
                    return new StreamedMemoryOwningRow<byte>(memoryOwner, bytesRead, (byte)_seperator);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static bool ByteIsNewLine(byte b)
        {
            return b == (byte)'\n' || b == (byte)'\r';
        }
    }
}

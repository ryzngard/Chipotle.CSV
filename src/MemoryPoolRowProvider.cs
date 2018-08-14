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

        public enum LineParser
        {
            Default,
            Streamed
        }

        public struct Configuration
        {
            public RowParseMechanism RowParseMechanism { get; set; }
            public LineParser LineParser { get; set; }
        }

        private static Configuration DefaultConfiguration = new Configuration()
        {
            RowParseMechanism = RowParseMechanism.Upfront,
            LineParser = LineParser.Default
        };

        public bool Completed { get; private set; }

        private bool _disposed = false;
        private bool _finishedReading = false;

        private Configuration _config;

        private readonly Stream _stream;
        private readonly char _seperator;

        private IDisposable _lineReader;

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

            Task.Run(() => ReadStream());
        }

        public void Dispose()
        {
            _disposed = true;
            _lineReader?.Dispose();
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
            switch (_config.LineParser)
            {
                case LineParser.Default:
                    {
                        var lineParser = new MemoryPoolLineParser(_stream, _queue, (byte)_seperator, _config.RowParseMechanism);
                        _lineReader = lineParser;

                        lineParser.Read();
                        break;
                    }

                case LineParser.Streamed:
                    {
                        var lineParser = new StreamLineParser(_stream);
                        _lineReader = lineParser;
                        foreach (var lineChunk in lineParser)
                        {
                            var row = Helpers.CreateRow(_config.RowParseMechanism, lineChunk.Value.MemoryOwner, lineChunk.Value.Memory, (byte)_seperator);
                            _queue.Enqueue(row);
                        }
                        break;
                    }

                default:
                    throw new InvalidOperationException();
            }

            _finishedReading = true;
        }
    }
}

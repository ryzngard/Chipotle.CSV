using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class Csv : IDisposable
    {
        public IEnumerable<string> Headers { get; private set; }
        public char Seperator { get; } = ',';

        private readonly Pipe _pipe;
        private readonly FileStream _stream;

        private bool _hasRead = false;
        private bool _disposed = false;
        private bool _complete = false;

        public bool Completed => _hasRead && _complete;

        protected Csv(FileStream stream, Pipe pipe)
        {
            _pipe = pipe;
            _stream = stream;
        }

        public async Task<Row<byte>> GetNextAsync()
        {
            if (_complete)
            {
                throw new InvalidOperationException("Cannot enumerate multiple times");
            }

            if (_disposed)
            {
                throw new InvalidOperationException("Object already disposed");
            }

            if (!_hasRead)
            {
                FillPipeAsync(_stream, _pipe.Writer);
                _hasRead = true;
            }

            var reader = _pipe.Reader;

            SequencePosition? position = null;
            ReadOnlySequence<byte> buffer;
            ReadResult result;

            do
            {
                result = await reader.ReadAsync();

                buffer = result.Buffer;
                position = buffer.PositionOf((byte)'\n');
            } while (position == null && !result.IsCompleted);

            // Tell the PipeReader how much of the buffer we have consumed
            reader.AdvanceTo((SequencePosition)position);

            // Stop reading if there's no more data coming
            if (result.IsCompleted)
            {
                _complete = true;

                // Mark the PipeReader as complete
                reader.Complete();
            }

            if (position == null && buffer.Length == 0)
            {
                throw new EntryPointNotFoundException("Unable to find another line");
            }

            return new Row<byte>(buffer, (byte)Seperator);
        }

        public static Csv Parse(FileStream stream)
        {
            var pipe = new Pipe();
            return new Csv(stream, pipe);
        }

        private static async Task FillPipeAsync(FileStream stream, PipeWriter writer)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                var memory = writer.GetMemory(minimumBufferSize);

                try
                {
                    int bytesRead = 0;
                    while (bytesRead < minimumBufferSize && stream.Position < stream.Length)
                    {
                        memory.Span[bytesRead++] = (byte)stream.ReadByte();
                    }

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    writer.Advance(bytesRead);

                }
                catch (Exception)
                {
                    break;
                }

                // Make the data available to the PipeReader
                FlushResult result = await writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
            }

            writer.Complete();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
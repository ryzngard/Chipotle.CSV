using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Stream _stream;

        private bool _hasRead = false;
        private bool _disposed = false;
        private bool _complete = false;

        public bool Completed => _hasRead && _complete;

        protected Csv(Stream stream, Pipe pipe)
        {
            _pipe = pipe;
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public async Task<Row<byte>> GetNextAsync()
        {
            if (_complete)
            {
                return null;
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
                position = buffer.PositionOf((byte)'\r');

                if (position == null)
                {
                    foreach (var b in buffer.ToArray())
                    {
                        Debug.WriteLine($"{b} = {System.Text.Encoding.ASCII.GetString(new[] { b })}");
                    }
                    reader.AdvanceTo(buffer.Start);
                }

            } while (position == null && !result.IsCompleted);

            if (position != null)
            {
                // Tell the PipeReader how much of the buffer we have consumed
                reader.AdvanceTo((SequencePosition)position);
            }

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

        public static Csv Parse(Stream stream)
        {
            var pipe = new Pipe();
            return new Csv(stream, pipe);
        }

        private static async Task FillPipeAsync(Stream stream, PipeWriter writer)
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
                catch (Exception e)
                {
                    writer.Complete(e);
                    return;
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
            this._disposed = true;
        }
    }
}
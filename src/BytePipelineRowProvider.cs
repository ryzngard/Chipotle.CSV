using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    /// <summary>
    /// Reads a stream using System.IO.Pipelines. The first row retrieval
    /// kicks off the writer thread that reads bytes from the underlying
    /// stream and writes them to the pipeline.
    /// </summary>
    public class BytePipelineRowProvider : IRowProvider
    {
        private bool _disposed = false;
        private bool _hasRead = false;

        private readonly char _seperator;
        private readonly Pipe _pipe;
        private readonly Stream _stream;

        public bool Completed { get; private set; } = false;

        public BytePipelineRowProvider(Stream stream, char seperator = ',')
        {
            this._seperator = seperator;
            this._pipe = new Pipe();
            this._stream = stream;
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
                    reader.AdvanceTo(buffer.Start);
                }

            } while (position == null && !result.IsCompleted);

            if (position != null)
            {
                buffer = buffer.Slice(0, position.Value);

                // Tell the PipeReader how much of the buffer we have consumed
                SequencePosition consumedPosition = position.Value;
                int offset = 1;
                while (true)
                {
                    consumedPosition = result.Buffer.GetPosition(offset, position.Value);

                    if (result.Buffer.TryGet(ref consumedPosition, out ReadOnlyMemory<byte> m, advance: false))
                    {
                        if (m.Span.Length == 0)
                        {
                            break;
                        }

                        if (m.Span[0] == '\r' || m.Span[0] == '\n')
                        {
                            offset++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                reader.AdvanceTo(consumedPosition);

            }

            // Stop reading if there's no more data coming
            else if (result.IsCompleted)
            {
                Completed = true;

                // Mark the PipeReader as complete
                reader.Complete();
            }
            else if (position == null && buffer.Length == 0)
            {
                throw new EntryPointNotFoundException("Unable to find another line");
            }

            return new Row<byte>(buffer, (byte)_seperator);
        }

        private async Task FillPipeAsync(Stream stream, PipeWriter writer)
        {
            const int minimumBufferSize = 512;

            while (!this._disposed)
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

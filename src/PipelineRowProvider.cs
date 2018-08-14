using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class PipelineRowProvider : IRowProvider
    {
        private bool _disposed = false;
        private bool _hasRead = false;

        private readonly char _seperator;
        private readonly Pipe _pipe;
        private readonly Stream _stream;

        public bool Completed { get; private set; } = false;

        public PipelineRowProvider(Stream stream, char seperator = ',')
        {
            this._seperator = seperator;
            this._pipe = new Pipe();
            this._stream = stream;
        }

        public void Dispose()
        {
            this._disposed = true;
        }

        public async Task<IRow<byte>> GetNextAsync()
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
                _hasRead = true;
                FillPipeAsync(_stream, _pipe.Writer);
            }

            var reader = _pipe.Reader;

            var readResult = await reader.ReadAsync();

            if (readResult.IsCompleted)
            {
                Completed = true;
            }

            if (readResult.Buffer.Length == 0)
            {
                return null;
            }

            return new Row<byte>(readResult.Buffer, (byte)_seperator);
        }

        private async Task FillPipeAsync(Stream stream, PipeWriter writer)
        {
            try
            {
                var memory = writer.GetMemory();
                int bytesRead = 0;
                bool consumeNewLines = false;
                while (!_disposed)
                {
                    // Read to the end of the line in the stream
                    int b = stream.ReadByte();

                    if (b == -1)
                    {
                        break;
                    }

                    if (consumeNewLines)
                    {
                        while (ByteIsNewLine((byte)b))
                        {
                            b = stream.ReadByte();

                            if (b == -1)
                            {
                                break;
                            }
                        }

                        consumeNewLines = false;
                    }

                    memory.Span[bytesRead] = (byte)b;

                    if (ByteIsNewLine((byte)b))
                    {
                        // Current memory represents a row. Write it to be read by the reader.
                        // Memory should be trimmed to only number of bytes read. It may have 
                        // been overallocated
                        await writer.WriteAsync(memory.Slice(0, bytesRead));
                        bytesRead = 0;
                        memory = writer.GetMemory();

                        // Make the data available to the PipeReader
                        FlushResult result = await writer.FlushAsync();

                        if (result.IsCompleted)
                        {
                            break;
                        }

                        consumeNewLines = true;

                    }
                    else if (++bytesRead >= memory.Length)
                    {
                        // The current allocated memory has run out of space, need to relocate
                        var tmpMemory = writer.GetMemory(memory.Length * 2);
                        memory.CopyTo(tmpMemory);
                        memory = tmpMemory;
                    }
                }

                if (bytesRead != 0)
                {
                    // Write the remaining bytes
                    await writer.WriteAsync(memory.Slice(0, bytesRead));
                }

                writer.Complete();
            }
            catch(Exception e)
            {
                writer.Complete(e);
            }
        }

        private static bool ByteIsNewLine(byte b)
        {
            return b == (byte)'\n' || b == (byte)'\r';
        }
    }
}

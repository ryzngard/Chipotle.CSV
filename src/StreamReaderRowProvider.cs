using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class StreamReaderRowProvider : IRowProvider
    {
        private readonly StreamReader _streamReader;
        private readonly char _seperator;
        private bool _disposed = false;

        public StreamReaderRowProvider(StreamReader streamReader, char seperator = ',')
        {
            _streamReader = streamReader;
            _seperator = seperator;
        }

        public bool Completed { get; private set; }

        public void Dispose()
        {
            _disposed = true;
        }

        public async Task<IRow<byte>> GetNextAsync()
        {
            if (_disposed)
            {
                throw new InvalidOperationException();
            }

            if (Completed)
            {
                return null;
            }

            var line = await _streamReader.ReadLineAsync();

            if (line == null)
            {
                Completed = true;
                return null;
            }

            return new StringRow(line, (byte)_seperator);
        }
    }
}

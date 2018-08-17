using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class StreamReaderTokenizer : ITokenizer
    {
        private readonly StreamReader _stream;
        private readonly bool _disposeStream;
        private readonly char _seperator;
        private bool _disposed = false;

        public StreamReaderTokenizer(StreamReader streamReader, char seperator = ',', bool disposeStream = false)
        {
            _stream = streamReader;
            _disposeStream = disposeStream;
            _seperator = seperator;
        }

        ~StreamReaderTokenizer()
        {
            Dispose(false);
        }

        public bool Completed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<ISection> GetNextAsync()
        {
            if (_disposed)
            {
                throw new InvalidOperationException();
            }

            if (_stream.EndOfStream)
            {
                return null;
            }

            var line = await _stream.ReadLineAsync();
            return new StringSection(line.Split(_seperator));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_disposeStream)
                {
                    _stream?.Dispose();
                }
            }

            _disposed = true;
        }
    }
}

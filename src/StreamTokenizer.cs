using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public abstract class StreamTokenizer : ITokenizer
    {
        protected readonly TokenizerSettings Settings;
        protected readonly Stream Stream;

        private static TokenizerSettings DefaultSettings = new TokenizerSettings(
            new char[] { '\n', '\r' },
            new char[] { ',' },
            Encoding.UTF8,
            false);

        public StreamTokenizer(Stream stream, TokenizerSettings? settings = null)
        {
            this.Stream = stream;
            this.Settings = settings ?? DefaultSettings;
        }

        ~StreamTokenizer()
        {
            Dispose(false);
        }

        public bool Completed { get; protected set; }
        public abstract Task<ISection> GetNextAsync();
        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Settings.DisposeStream)
                {
                    Stream?.Dispose();
                }
            }

            Disposed = true;
        }

        // TODO: Use Expression body
        protected bool IsSectionDelimiter(byte value)
        {
            return Settings.SectionDelimiters.Any(b => b == value);
        }

        protected bool IsSegmentDelimiter(byte value)
        {
            return Settings.SegmentDelimiters.Any(b => b == value);
        }

    }
}

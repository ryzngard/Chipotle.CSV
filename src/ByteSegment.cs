using System;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    internal class ByteSegment : ISegment<byte>
    {
        private readonly Encoding _encoding;
        public ByteSegment(ReadOnlyMemory<byte> memory, Encoding encoding)
        {
            this.Value = memory;
            this._encoding = encoding;
        }

        ~ByteSegment()
        {
            Dispose(false);
        }

        public ReadOnlyMemory<byte> Value { get; private set; }

        public override string ToString()
        {
            if (Value.Length == 0)
            {
                return string.Empty;
            }

            if (_encoding == null)
            {
                throw new InvalidOperationException("No encoding available to encode bytes");
            }

            return _encoding.GetString(Value.ToArray());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Value = null;
            }
        }
    }
}

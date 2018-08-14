using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chipotle.CSV
{
    class StringRow : IRow<byte>
    {
        private bool _disposed = false;
        private readonly byte[] _row;
        private readonly ReadOnlyMemory<byte> _rowMemory;
        private IList<ReadOnlyMemory<byte>> _data;

        public StringRow(string row, byte seperator)
        {
            _row = row.Select(c => (byte)c).ToArray();
            _rowMemory = _row.AsMemory();
            _data = new List<ReadOnlyMemory<byte>>();

            int offset = 0;
            int length = 0;

            foreach (var c in _rowMemory.Span)
            {
                // Split the buffer by the seperator, storing each chunk
                // as a value
                if (c == seperator)
                {
                    var lineBuffer = _rowMemory.Slice(offset, length);
                    _data.Add(lineBuffer);
                    offset = offset + length + 1;
                    length = 0;
                }
                else
                {
                    length++;
                }
            }

            // Add any remaining buffer as the final value
            if (offset < _rowMemory.Length)
            {
                var lineBuffer = _rowMemory.Slice(offset);
                _data.Add(lineBuffer);
            }
        }

        public ReadOnlySpan<byte> this[int index] => _data[index].Span;

        public ReadOnlySpan<byte> this[string key] => throw new NotImplementedException();

        public void Dispose()
        {
            _disposed = true;
        }

        public IEnumerator<ReadOnlyMemory<byte>> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

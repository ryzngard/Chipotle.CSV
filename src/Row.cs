using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chipotle.CSV
{
    public class Row<T> : IEnumerable<ReadOnlyMemory<T>>
    {
        private IList<ReadOnlyMemory<T>> _values;
        private IDictionary<string, int> _headers;

        internal Row(ReadOnlySequence<T> buffer, T seperator, IDictionary<string, int> headers = null)
        {
            _headers = headers;
            _values = new List<ReadOnlyMemory<T>>(_headers?.Keys.Count ?? 10);

            // Split by the seperator
            int offset = 0;
            int length = 1;
            foreach (var b in buffer)
            {
                if (b.Span[0] == seperator)
                {
                    var lineBuffer = buffer.Slice(offset, length);

                    _values.Add(new Memory<T>(lineBuffer.ToArray()));
                }
            }
        }

        
        public ReadOnlySpan<T> this[string key] => _getValueFromKey(key);

        public ReadOnlySpan<T> this[int index] => _values[index].Span;

        private ReadOnlySpan<T> _getValueFromKey(string key)
        {
            if (_headers == null || !_headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"'{key}' is invalid");
            }

            return this[_headers[key]];
        }

        public IEnumerator<ReadOnlyMemory<T>> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}
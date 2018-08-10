using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chipotle.CSV
{
    public class Row<T> : IEnumerable<ReadOnlyMemory<T>>
    {
        private const int defaultListSize = 8;

        private IList<ReadOnlyMemory<T>> _values;
        private IDictionary<string, int> _headers;

        internal Row(ReadOnlySequence<T> buffer, T seperator, IDictionary<string, int> headers = null)
        {
            _headers = headers;
            _values = new List<ReadOnlyMemory<T>>(_headers?.Keys.Count ?? defaultListSize);

            // Split by the seperator
            int offset = 0;
            int length = 1;
            foreach (var b in buffer)
            {
                if (b.Span.Length == 0)
                {
                    Debug.WriteLine($"Skipping byte in buffer :: {b}");
                    continue;
                }

                if (b.Span[0]?.Equals(seperator) == true)
                {
                    var lineBuffer = buffer.Slice(offset, length);

                    _values.Add(new Memory<T>(lineBuffer.ToArray()));
                }
            }
        }

        
        public ReadOnlySpan<T> this[string key] => _getValueFromKey(key);

        public ReadOnlySpan<T> this[int index] => _values[index].Span;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var v in _values)
            {
                sb.Append($"{v},");
            }

            return sb.ToString();
        }

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
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    internal class Row<T> : IRow<T>
    {
        private const int defaultListSize = 8;

        private IList<ReadOnlyMemory<T>> _values;
        private IDictionary<string, int> _headers;
        private Action _delayParseAction;

        internal Row(ReadOnlyMemory<T> readOnlyMemory, T seperator, IDictionary<string, int> headers = null)
        {
            _headers = headers;
            _values = new List<ReadOnlyMemory<T>>(_headers?.Keys.Count ?? defaultListSize);

            _delayParseAction = new Action(() =>
            {
                Parse(new ReadOnlySequence<T>(readOnlyMemory), seperator);
            });
        }

        internal Row(ReadOnlySequence<T> buffer, T seperator, IDictionary<string, int> headers = null)
        {
            _headers = headers;
            _values = new List<ReadOnlyMemory<T>>(_headers?.Keys.Count ?? defaultListSize);

            Parse(buffer, seperator);
        }

        private void Parse(ReadOnlySequence<T> buffer, T seperator)
        {
            // Split by the seperator
            int offset = 0;
            int length = 0;
            foreach (var segment in buffer)
            {
                foreach (var b in segment.Span)
                {
                    // Split the buffer by the seperator, storing each chunk
                    // as a value
                    if (b?.Equals(seperator) == true)
                    {
                        var lineBuffer = buffer.Slice(offset, length);
                        addToValues(lineBuffer);
                        offset = offset + length + 1;
                        length = 0;
                    }
                    else
                    {
                        length++;
                    }
                }
            }

            // Add any remaining buffer as the final value
            if (offset < buffer.Length)
            {
                var lineBuffer = buffer.Slice(offset);
                addToValues(lineBuffer);
            }

            void addToValues(ReadOnlySequence<T> sequence)
            {
                var start = sequence.Start;
                if (!sequence.TryGet(ref start, out ReadOnlyMemory<T> memory))
                {
                    throw new Exception($"Encountered an exception reading the memory");
                }

                _values.Add(memory);
            }
        }
        
        public ReadOnlySpan<T> this[string key] => _getValueFromKey(key);

        public ReadOnlySpan<T> this[int index] => _getValue(index);

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

        private ReadOnlySpan<T> _getValue(int index)
        {
            EnsureValuesInitialized();

            return _values[index].Span;
        }

        public IEnumerator<ReadOnlyMemory<T>> GetEnumerator()
        {
            EnsureValuesInitialized();

            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureValuesInitialized();

            return _values.GetEnumerator();
        }


        private void EnsureValuesInitialized()
        {
            if (_delayParseAction != null)
            {
                _delayParseAction();
                _delayParseAction = null;
            }
        }
        public virtual void Dispose()
        {
        }
    }
}
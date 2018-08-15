using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    class StreamedMemoryOwningRow<T> : IRow<T>
    {
        private readonly IMemoryOwner<T> _memoryOwner;
        private readonly ReadOnlyMemory<T> _memory;
        private readonly T _seperator;
        private bool _disposed = false;
        private readonly IDictionary<string, int> _headers;

        private IEnumerator<ReadOnlyMemory<T>> _enumerator;
        private byte seperator;

        public StreamedMemoryOwningRow(IMemoryOwner<T> memoryOwner, int bytesRead, T seperator, IDictionary<string, int> headers = null) :
            this(memoryOwner, memoryOwner.Memory.Slice(0, bytesRead), seperator, headers)
        {
        }

        public StreamedMemoryOwningRow(IMemoryOwner<T> memoryOwner, ReadOnlyMemory<T> memory, T seperator, IDictionary<string, int> headers = null)
        {
            _memoryOwner = memoryOwner;
            _memory = memory;
            _seperator = seperator;
            _headers = headers;

            _enumerator = GetEnumerator();
        }

        public ReadOnlySpan<T> this[int index]
        {
            get
            {
                if (_disposed)
                {
                    throw new InvalidOperationException();
                }
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                _enumerator.Reset();

                int count = -1;
                while(++count <= index && _enumerator.MoveNext())
                {
                }

                if (count < index)
                {
                    throw new IndexOutOfRangeException();
                }

                return _enumerator.Current.Span;
            }
        }

        public ReadOnlySpan<T> this[string key] => this[_headers?[key] ?? throw new InvalidOperationException()];

        public void Dispose()
        {
            _disposed = true;
            _memoryOwner.Dispose();
            _enumerator.Dispose();
        }

        public IEnumerator<ReadOnlyMemory<T>> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<ReadOnlyMemory<T>>
        {
            private StreamedMemoryOwningRow<T> _owner;
            private bool _disposed = false;
            private int _startIndex = 0;

            internal Enumerator(StreamedMemoryOwningRow<T> owner)
            {
                _owner = owner;
            }

            public ReadOnlyMemory<T> Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                Reset();
                _disposed = true;
            }

            public bool MoveNext()
            {
                if (_disposed)
                {
                    throw new InvalidOperationException();
                }

                // Reset current value to null. Will be set again if found
                Current = null;

                if (_startIndex >= _owner._memory.Length)
                {
                    return false;
                }

                // Find the next instance of the seperator 
                var index = _startIndex;

                while (index < _owner._memory.Length)
                {
                    var value = _owner._memory.Span[index++];

                    if (value?.Equals(_owner._seperator) == true)
                    {
                        Current = _owner._memory.Slice(_startIndex, index - _startIndex - 1);

                        _startIndex = index;
                        return true;
                    }
                }

                // If we made it to the end, set to what we have and put the index at the end
                Current = _owner._memory.Slice(_startIndex);
                _startIndex = _owner._memory.Length;
                return true;
            }

            public void Reset()
            {
                if (_disposed)
                {
                    throw new InvalidOperationException();
                }

                Current = null;
                _startIndex = 0;
            }
        }
    }
}

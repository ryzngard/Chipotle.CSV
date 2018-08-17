using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chipotle.CSV
{
    internal class MemoryOwningSection : ISection
    {
        private static Memory<byte> Empty = new Memory<byte>();

        private readonly int[] _segmentHints;
        private readonly Encoding _encoding;
        private readonly IMemoryOwner<byte> _memoryOwner;

        public MemoryOwningSection(IMemoryOwner<byte> memoryOwner, List<int> segmentHints, Encoding encoding)
        {
            _encoding = encoding;
            _segmentHints = segmentHints.ToArray();
            _memoryOwner = memoryOwner;
        }

        ~MemoryOwningSection()
        {
            Dispose(false);
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
                _memoryOwner?.Dispose();
            }
        }

        public ISegment this[int index]
        {
            get
            {
                if (index > _segmentHints.Length || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                int pivot = 0;
                var memory = this.Memory;

                if (index == _segmentHints.Length)
                {
                    if (index == 0)
                    {
                        return new ByteSegment(Memory, _encoding);
                    }

                    pivot = _segmentHints[index - 1];
                    if (Memory.Length == pivot)
                    {
                        return new ByteSegment(Empty, _encoding);
                    }

                    return new ByteSegment(Memory.Slice(pivot + 1), _encoding);
                }

                pivot = _segmentHints[index];

                if (index == 0)
                {
                    return new ByteSegment(Memory.Slice(0, pivot), _encoding);
                }

                var prevPivot = _segmentHints[index - 1];
                return new ByteSegment(Memory.Slice(prevPivot + 1, pivot - prevPivot - 1), _encoding);
            }
        }

        protected virtual Memory<byte> Memory => _memoryOwner.Memory;
        public ISegment this[string key] => throw new NotImplementedException();

        public IEnumerator<ISegment> GetEnumerator()
        {
            return Enumerable.Range(0, _segmentHints.Length + 1)
                    .Select(i => this[i])
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

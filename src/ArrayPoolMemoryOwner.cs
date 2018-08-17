using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chipotle.CSV
{
    class ArrayPoolMemoryOwner : IMemoryOwner<byte>
    {
        private byte[] _rentedMemory;
        private readonly int _memorySize;

        public ArrayPoolMemoryOwner(List<ReadOnlyMemory<byte>> memorySegments, int memorySize)
        {
            _rentedMemory = ArrayPool<byte>.Shared.Rent(memorySize);
            _memorySize = memorySize;

            int memoryOffset = 0;
            var memory = this.Memory;
            foreach (var segment in memorySegments)
            {
                segment.CopyTo(memory.Slice(memoryOffset, segment.Length));
                memoryOffset += segment.Length;
            }
        }

        public Memory<byte> Memory => new Memory<byte>(GetArray(), 0, _memorySize);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            var memory = Interlocked.Exchange(ref _rentedMemory, null);
            if (memory != null)
            {
                ArrayPool<byte>.Shared.Return(memory);
            }
        }

        private byte[] GetArray()
        {
            return Interlocked.CompareExchange(ref _rentedMemory, null, null)
                ?? throw new ObjectDisposedException(ToString());
        }
    }
}

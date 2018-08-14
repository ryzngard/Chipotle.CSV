using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    internal class MemoryOwningRow<T> : Row<T>
    {
        private readonly IMemoryOwner<T> _memoryOwner;
        private IMemoryOwner<byte> memoryOwner;
        private ReadOnlyMemory<byte> memory;
        private byte seperator;

        public MemoryOwningRow(IMemoryOwner<T> memoryOwner, ReadOnlyMemory<T> memory, T seperator) :
            base(memory, seperator, null)
            
        {
            _memoryOwner = memoryOwner;
        }

        public MemoryOwningRow(IMemoryOwner<T> memoryOwner,  int informationSize, T seperator, IDictionary<string, int> headers = null) :
            base(memoryOwner.Memory.Slice(0, informationSize), seperator, headers)
        {
            _memoryOwner = memoryOwner;
        }

        public override void Dispose()
        {
            base.Dispose();
            _memoryOwner.Dispose();
        }
    }
}

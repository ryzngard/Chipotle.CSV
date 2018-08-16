using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    public interface IMemoryProvider<T>
    {
        int MaxNeededMemory { get; }
        void SubmitChunkUsage(int chunkSize);

        IMemoryOwner<T> Rent();
        IMemoryOwner<T> Rent(IMemoryOwner<T> existingMemory, int startOffset);
        IMemoryOwner<T> Rent(IMemoryOwner<T> existingMemory, int startOffset, int length);
    }
}

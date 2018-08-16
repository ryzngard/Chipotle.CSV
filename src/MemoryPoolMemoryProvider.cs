using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    class MemoryPoolMemoryProvider : IMemoryProvider<byte>
    {
        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        private int _chunkSize;

        public int MaxNeededMemory { get; }

        public MemoryPoolMemoryProvider(int maxNeeded)
        {
            MaxNeededMemory = maxNeeded;
            _chunkSize = Math.Min(maxNeeded, _memoryPool.MaxBufferSize);
        }

        public IMemoryOwner<byte> Rent()
        {
            return _memoryPool.Rent(_chunkSize);
        }

        public IMemoryOwner<byte> Rent(IMemoryOwner<byte> existingMemory, int startOffset)
        {
            return Rent(existingMemory, startOffset, existingMemory.Memory.Length - startOffset);
        }

        public IMemoryOwner<byte> Rent(IMemoryOwner<byte> existingMemory, int startOffset, int length)
        {
            var toCopy = existingMemory.Memory.Slice(startOffset, length);
            var rentSize = _chunkSize > toCopy.Length ? _chunkSize : Math.Min(toCopy.Length * 2, _memoryPool.MaxBufferSize);
            var memory = _memoryPool.Rent(rentSize);
            toCopy.CopyTo(memory.Memory);

            return memory;
        }

        public void SubmitChunkUsage(int chunkSize)
        {
            // For now, just keep the max chunk size. Potentially later
            // data can be used to adjust memory renting
            _chunkSize = Math.Max(chunkSize, _chunkSize);
        }
    }
}

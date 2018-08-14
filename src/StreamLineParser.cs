using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chipotle.CSV
{
    class StreamLineParser : IEnumerable<StreamLineParser.MemoryChunk?>, IEnumerator<StreamLineParser.MemoryChunk?>
    {
        public struct MemoryChunk
        {
            public IMemoryOwner<byte> MemoryOwner { get; set; }
            public ReadOnlyMemory<byte> Memory { get; set; }
        }

        private readonly Stream _stream;
        private IMemoryOwner<byte> _chunk;
        private int _offset;
        private int _chunkSize = 2048;
        private int _chunkIndex = 0;
        private int _validMemoryIndex = -1;
        private bool _previousWasNewLine = false;

        private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;

        public StreamLineParser(Stream stream)
        {
            _stream = stream;
        }

        public MemoryChunk? Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Reset();
        }

        public IEnumerator<MemoryChunk?> GetEnumerator()
        {
            return this;
        }

        private int ResizeChunk()
        {
            int startIndex = 0;
            if (_chunk == null)
            {
                _chunk = _memoryPool.Rent(_chunkSize);
            }
            else
            {
                startIndex = _chunk.Memory.Length;
                var tmp = _memoryPool.Rent(startIndex + _chunkSize);
                _chunk.Memory.CopyTo(tmp.Memory);

                _chunk = tmp;
            }

            // Fill in the new memory from the stream
            byte[] newMemory = new byte[_chunk.Memory.Length - startIndex];

            var bytesRead = _stream.Read(newMemory, _offset, newMemory.Length);

            for (int i = 0; i < bytesRead; i++)
            {
                _chunk.Memory.Span[i + startIndex] = newMemory[i];
            }

            return startIndex + bytesRead;
        }
        public bool MoveNext()
        {
            if (_chunkIndex == _validMemoryIndex)
            {
                return false;
            }

            if (_chunk == null)
            {
                _validMemoryIndex = ResizeChunk();
            }


            int i = _chunkIndex;

            while (true)
            {
                if (IsNewLine(_chunk.Memory.Span[i++]))
                {
                    if (_previousWasNewLine)
                    {
                        _chunkIndex++;
                    }
                    else
                    {
                        var size = i - _chunkIndex;

                        Current = new MemoryChunk()
                        {
                            MemoryOwner = _chunk,
                            Memory = _chunk.Memory.Slice(_chunkIndex, size - 1)
                        };

                        _chunkIndex = i;
                        _chunkSize = size;

                        _previousWasNewLine = true;

                        return true;
                    }
                }
                else
                {
                    _previousWasNewLine = false;
                }

                if (i == _validMemoryIndex)
                {
                    if (_validMemoryIndex == _chunk.Memory.Length)
                    {
                        _validMemoryIndex = ResizeChunk();
                    }

                    // If the memory index is the last valid index, we've reached
                    // the end of the stream
                    if (i == _validMemoryIndex)
                    {
                        if (_chunkIndex == _chunk.Memory.Length)
                        {
                            Current = null;
                            return false;
                        }

                        Current = new MemoryChunk()
                        {
                            MemoryOwner = _chunk,
                            Memory = _chunk.Memory.Slice(_chunkIndex, _validMemoryIndex - _chunkIndex)

                        };

                        _chunkIndex = _validMemoryIndex;

                        return true;
                    }
                }
            }
        }

        public void Reset()
        {
            Current = null;
            _chunk = null;
            _offset = 0;
            _chunkIndex = 0;
            _validMemoryIndex = -1;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static bool IsNewLine(byte b)
        {
            return b == (byte)'\r' || b == (byte)'\n';
        }
    }
}

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    internal static class Helpers
    {
        public static bool ByteIsNewLine(byte b)
        {
            return b == (byte)'\n' || b == (byte)'\r';
        }

        public static IRow<byte> CreateRow(MemoryPoolRowProvider.RowParseMechanism mechanism, IMemoryOwner<byte> memoryOwner, int bytesRead, byte seperator)
        {
            switch (mechanism)
            {
                case MemoryPoolRowProvider.RowParseMechanism.Upfront:
                    return new MemoryOwningRow<byte>(memoryOwner, bytesRead, seperator);

                case MemoryPoolRowProvider.RowParseMechanism.Streamed:
                    return new StreamedMemoryOwningRow<byte>(memoryOwner, bytesRead, seperator);

                default:
                    throw new InvalidOperationException();
            }
        }

        public static IRow<byte> CreateRow(MemoryPoolRowProvider.RowParseMechanism mechanism, IMemoryOwner<byte> memoryOwner, ReadOnlyMemory<byte> memory, byte seperator)
        {
            switch (mechanism)
            {
                case MemoryPoolRowProvider.RowParseMechanism.Upfront:
                    return new MemoryOwningRow<byte>(memoryOwner, memory, seperator);

                case MemoryPoolRowProvider.RowParseMechanism.Streamed:
                    return new StreamedMemoryOwningRow<byte>(memoryOwner, memory, seperator);

                default:
                    throw new InvalidOperationException();
            }
        }

    }
}

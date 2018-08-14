using System;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    public interface IRow<T> : IEnumerable<ReadOnlyMemory<T>>, IDisposable
    {
        ReadOnlySpan<T> this[int index] { get; }
        ReadOnlySpan<T> this[string key] { get; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV;

public interface ISegment : IDisposable
{
}

public interface ISegment<T> : ISegment
{
    ReadOnlyMemory<T> Value { get; }
}

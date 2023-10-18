using System;
using System.Collections.Generic;

namespace Chipotle.CSV;

public interface ISection : IEnumerable<ISegment>, IDisposable
{
    ISegment this[int index] { get; }
    ISegment this[string key] { get; }
}

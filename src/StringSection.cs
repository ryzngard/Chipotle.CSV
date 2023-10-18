using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chipotle.CSV;

internal class StringSection : ISection
{
    private readonly string[] _segments;

    public StringSection(string[] segments)
    {
        _segments = segments;
    }

    public ISegment this[int index] => new StringSegment(_segments[index]);

    public ISegment this[string key] => throw new NotImplementedException();

    public void Dispose()
    {
    }

    public IEnumerator<ISegment> GetEnumerator()
    {
        return Enumerable
            .Range(0, _segments.Length)
            .Select(i => this[i])
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

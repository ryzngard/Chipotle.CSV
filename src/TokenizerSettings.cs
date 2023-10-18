using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chipotle.CSV;

public class TokenizerSettings
{
    public byte[] SectionDelimiters { get; }
    public byte[] SegmentDelimiters { get; }
    public Encoding Encoding { get; }
    public bool DisposeStream { get; }

    public TokenizerSettings(IEnumerable<byte> sectionDelimiters, IEnumerable<byte> segmentDelimiters, Encoding encoding, bool disposeStream = false)
    {
        this.SectionDelimiters = sectionDelimiters.ToArray();
        this.SegmentDelimiters = segmentDelimiters.ToArray();
        this.Encoding = encoding;
        this.DisposeStream = DisposeStream;
    }

    public TokenizerSettings(byte sectionDelimiter, byte segmentDelimiter, Encoding encoding, bool disposeStream = false)
        : this(new byte[] { sectionDelimiter }, new byte[] { segmentDelimiter }, encoding, disposeStream)
    {
    }

    public TokenizerSettings(char sectionDelimiter, char segmentDelimiter, Encoding encoding, bool disposeStream = false)
        : this((byte)sectionDelimiter, (byte)segmentDelimiter, encoding, disposeStream)
    {
    }

    public TokenizerSettings(IEnumerable<char> sectionDelimiters, IEnumerable<char> segmentDelimiters, Encoding encoding, bool disposeStream = false)
        : this(sectionDelimiters.Select(c => (byte)c), segmentDelimiters.Select(c => (byte)c), encoding, disposeStream)
    {
    }
}

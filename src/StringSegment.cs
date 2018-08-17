using System;
using System.Collections.Generic;
using System.Text;

namespace Chipotle.CSV
{
    class StringSegment : ISegment<char>
    {
        private readonly string _value;
        public StringSegment(string value)
        {
            _value = value ?? string.Empty;
        }

        public ReadOnlyMemory<char> Value => _value.AsMemory();

        public override string ToString()
        {
            return _value;
        }

        public void Dispose()
        {
        }
    }
}

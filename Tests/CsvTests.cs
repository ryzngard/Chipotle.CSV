using Chipotle.CSV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class CsvTests
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        [Fact]
        public async Task BasicCsvParse()
        {
            using (var stream = GetStreamFromString("foo,bar,chunky,bacon"))
            using (var csv = Csv.Parse(stream))
            {
                var row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("foo", _encoding.GetString(row[0].ToArray()));
                Assert.Equal("bar", _encoding.GetString(row[1].ToArray()));
                Assert.Equal("chunky", _encoding.GetString(row[2].ToArray()));
                Assert.Equal("bacon", _encoding.GetString(row[3].ToArray()));
            }
        }

        [Fact]
        public async Task MultilineCsvParse()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("foo,bar,chunky,bacon");
            sb.AppendLine("bacon,is,very,chunky");

            for (int i = 0; i < 10000; i++)
            {
                sb.AppendLine("this,is,lots,of,lines");
            }

            using (var stream = GetStreamFromString(sb.ToString()))
            using (var csv = Csv.Parse(stream))
            {
                var row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("foo", _encoding.GetString(row[0].ToArray()));
                Assert.Equal("bar", _encoding.GetString(row[1].ToArray()));
                Assert.Equal("chunky", _encoding.GetString(row[2].ToArray()));
                Assert.Equal("bacon", _encoding.GetString(row[3].ToArray()));

                row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("bacon", _encoding.GetString(row[0].ToArray()));
                Assert.Equal("is", _encoding.GetString(row[1].ToArray()));
                Assert.Equal("very", _encoding.GetString(row[2].ToArray()));
                Assert.Equal("chunky", _encoding.GetString(row[3].ToArray()));

                int count = 2;
                do
                {
                    //Debug.WriteLine($"Trying to read line {++count}");
                    row = await csv.GetNextAsync();
                } while (row != null);
            }
        }

        private Stream GetStreamFromString(string str)
        {
            return new MemoryStream(_encoding.GetBytes(str));
        }
    }
}

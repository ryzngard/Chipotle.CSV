using Chipotle.CSV;
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
        public enum TokenizerType
        {
            Pipeline,
            String,
            ArrayPool
        }

        private ITokenizer GetTokenizer(TokenizerType rowProviderType, Stream stream)
        {
            switch(rowProviderType)
            {
                case TokenizerType.Pipeline: return new PipelineStreamTokenizer(stream);
                case TokenizerType.String: return new StreamReaderTokenizer(new StreamReader(stream));
                case TokenizerType.ArrayPool: return new MemoryManagedTokenizer(stream);
                default: throw new InvalidDataException();
            }
        }

        [Theory]
        [InlineData(TokenizerType.Pipeline)]
        [InlineData(TokenizerType.String)]
        [InlineData(TokenizerType.ArrayPool)]
        public async Task BasicCsvParse(TokenizerType rowProviderType)
        {
            using (var stream = GetStreamFromString("foo,bar,chunky,bacon"))
            using (var csv = GetTokenizer(rowProviderType, stream))
            {
                var row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("foo", row[0].ToString());
                Assert.Equal("bar", row[1].ToString());
                Assert.Equal("chunky", row[2].ToString());
                Assert.Equal("bacon", row[3].ToString());
            }
        }

        [Theory]
        [InlineData(TokenizerType.Pipeline)]
        [InlineData(TokenizerType.String)]
        [InlineData(TokenizerType.ArrayPool)]
        public async Task MultilineCsvParse(TokenizerType rowProviderType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("foo,bar,chunky,bacon");
            sb.AppendLine("bacon,is,very,chunky");

            for (int i = 0; i < 10000; i++)
            {
                sb.AppendLine("this,is,lots,of,lines");
            }

            using (var stream = GetStreamFromString(sb.ToString()))
            using (var csv = GetTokenizer(rowProviderType, stream))
            {
                var row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("foo", row[0].ToString());
                Assert.Equal("bar", row[1].ToString());
                Assert.Equal("chunky", row[2].ToString());
                Assert.Equal("bacon", row[3].ToString());

                row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("bacon", row[0].ToString());
                Assert.Equal("is", row[1].ToString());
                Assert.Equal("very", row[2].ToString());
                Assert.Equal("chunky", row[3].ToString());

                do
                {
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

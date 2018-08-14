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
        public enum RowProviderType
        {
            BytePipeline,
            Pipeline,
            MemoryPool,
            StreamedRowMemoryPool
        }

        private IRowProvider GetProvider(RowProviderType rowProviderType, Stream stream)
        {
            switch(rowProviderType)
            {
                case RowProviderType.BytePipeline: return new BytePipelineRowProvider(stream);
                case RowProviderType.Pipeline: return new PipelineRowProvider(stream);
                case RowProviderType.MemoryPool:
                    return new MemoryPoolRowProvider(stream, config: new MemoryPoolRowProvider.Configuration()
                    {
                        RowParseMechanism = MemoryPoolRowProvider.RowParseMechanism.Upfront
                    });

                case RowProviderType.StreamedRowMemoryPool:
                    return new MemoryPoolRowProvider(stream, config: new MemoryPoolRowProvider.Configuration()
                    {
                        RowParseMechanism = MemoryPoolRowProvider.RowParseMechanism.Streamed
                    });

                default: throw new InvalidDataException();
            }
        }

        [Theory]
        [InlineData(RowProviderType.BytePipeline)]
        [InlineData(RowProviderType.MemoryPool)]
        [InlineData(RowProviderType.StreamedRowMemoryPool)]
        public async Task BasicCsvParse(RowProviderType rowProviderType)
        {
            using (var stream = GetStreamFromString("foo,bar,chunky,bacon"))
            using (var csv = Csv.Parse(stream, GetProvider(rowProviderType, stream)))
            {
                var row = await csv.GetNextAsync();

                Assert.Equal(4, row.Count());

                Assert.Equal("foo", _encoding.GetString(row[0].ToArray()));
                Assert.Equal("bar", _encoding.GetString(row[1].ToArray()));
                Assert.Equal("chunky", _encoding.GetString(row[2].ToArray()));
                Assert.Equal("bacon", _encoding.GetString(row[3].ToArray()));
            }
        }

        [Theory]
        [InlineData(RowProviderType.BytePipeline)]
        [InlineData(RowProviderType.MemoryPool)]
        [InlineData(RowProviderType.StreamedRowMemoryPool)]
        public async Task MultilineCsvParse(RowProviderType rowProviderType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("foo,bar,chunky,bacon");
            sb.AppendLine("bacon,is,very,chunky");

            for (int i = 0; i < 10000; i++)
            {
                sb.AppendLine("this,is,lots,of,lines");
            }

            using (var stream = GetStreamFromString(sb.ToString()))
            using (var csv = Csv.Parse(stream, GetProvider(rowProviderType, stream)))
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

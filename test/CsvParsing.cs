using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using System;
using System.IO;
using System.Threading.Tasks;

namespace test
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvParsing
    {
        private readonly Stream _fileStream;

        public CsvParsing()
        {
            var assembly = typeof(CsvParsing).Assembly;
            this._fileStream = assembly.GetManifestResourceStream("FL_insurance_sample.csv");
        }

        private byte[] getStreamBytes()
        {
            byte[] bytes = new byte[_fileStream.Length];

            _fileStream.Read(bytes, 0, (int)_fileStream.Length);
            _fileStream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }
        [Benchmark]
        public async Task<Csv> ParseCsv()
        {
            var csv = Csv.Parse(_fileStream);
                
            var row = await csv.GetNextAsync();

            while (row != null)
            {
                row = await csv.GetNextAsync();
            }

            return csv;
        }
    }
}

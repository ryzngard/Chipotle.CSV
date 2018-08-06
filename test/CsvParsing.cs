using BenchmarkDotNet.Attributes;
using System;
using System.IO;

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
        public Csv ParseCsv()
        {
            
        }
    }
}

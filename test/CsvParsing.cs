using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace test
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvParsing
    {
        public CsvParsing()
        {
        }

        private static Stream GetStream(string name)
        {
            var assembly = typeof(CsvParsing).Assembly;
            return assembly.GetManifestResourceStream($"test.{name}");
        }

        [Benchmark]
        public async Task<Csv> Parse2KB()
        {
            return await ParseCsvFile("2KB.csv");
        }

        [Benchmark]
        public async Task<Csv> Parse4KB()
        {
            return await ParseCsvFile("4KB.csv");
        }

        [Benchmark]
        public async Task<Csv> Parse8KB()
        {
            return await ParseCsvFile("8KB.csv");
        }

        [Benchmark]
        public async Task<Csv> Parse1MB()
        {
            return await ParseCsvFile("Import_User_Sample_en_Duplicated.csv");
        }

        [Benchmark]
        public async Task<Csv> Parse4MB()
        {
            return await ParseCsvFile("FL_insurance_sample.csv");
        }

        private static async Task<Csv> ParseCsvFile(string name)
        {
            using (var stream = GetStream(name))
            using (var csv = Csv.Parse(stream))
            {
                Debug.WriteLine($"Reading file, size = {stream.Length}");

                Row<byte> row;
                int count = 0;
                do
                {
                    row = await csv.GetNextAsync();
                    count++;
                }
                while (row != null);

                Debug.WriteLine($"Read {count} rows");

                return csv;
            }
        }
    }
}

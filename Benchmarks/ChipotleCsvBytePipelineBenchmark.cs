using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class ChipotleCsvBytePipelineBenchmark
    {
        public ChipotleCsvBytePipelineBenchmark()
        {
        }

        [Benchmark]
        public async Task<Csv> Parse2KB()
        {
            return await ParseCsvFile(Resources.FileSize.KB2);
        }

        [Benchmark]
        public async Task<Csv> Parse4KB()
        {
            return await ParseCsvFile(Resources.FileSize.KB4);
        }

        [Benchmark]
        public async Task<Csv> Parse8KB()
        {
            return await ParseCsvFile(Resources.FileSize.KB8);
        }

        [Benchmark]
        public async Task<Csv> Parse16KB()
        {
            return await ParseCsvFile(Resources.FileSize.KB16);
        }

        [Benchmark]
        public async Task<Csv> Parse32KB()
        {
            return await ParseCsvFile(Resources.FileSize.KB32);
        }

        //[Benchmark]
        //public async Task<Csv> Parse1MB()
        //{
        //    return await ParseCsvFile("Import_User_Sample_en_Duplicated.csv");
        //}

        [Benchmark]
        public async Task<Csv> Parse4MB()
        {
            return await ParseCsvFile(Resources.FileSize.MB4);
        }

        private static async Task<Csv> ParseCsvFile(Resources.FileSize size)
        {
            using (var stream = Resources.GetStream(size))
            using (var csv = Csv.Parse(stream))
            {
                Debug.WriteLine($"Reading file, size = {stream.Length}");

                IRow<byte> row;
                int count = 0;
                while (true)
                {
                    row = await csv.GetNextAsync();

                    if (row == null)
                    {
                        break;
                    }

                    count++;
                }

                Debug.WriteLine($"Read {count} rows");

                return csv;
            }
        }
    }
}

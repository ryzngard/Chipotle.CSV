using BenchmarkDotNet.Attributes;
using CsvHelper;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvHelperBenchmark
    {
        [Benchmark]
        public Task<CsvReader> Parse2KB()
        {
            return ParseCsvFile(Resources.FileSize.KB2);
        }

        [Benchmark]
        public Task<CsvReader> Parse4KB()
        {
            return ParseCsvFile(Resources.FileSize.KB4);
        }

        [Benchmark]
        public Task<CsvReader> Parse8KB()
        {
            return ParseCsvFile(Resources.FileSize.KB8);
        }

        [Benchmark]
        public Task<CsvReader> Parse16KB()
        {
            return ParseCsvFile(Resources.FileSize.KB16);
        }

        [Benchmark]
        public Task<CsvReader> Parse32KB()
        {
            return ParseCsvFile(Resources.FileSize.KB32);
        }

        //[Benchmark]
        //public Task<CsvReader> Parse1MB()
        //{
        //    return ParseCsvFile("Import_User_Sample_en_Duplicated.csv");
        //}

        [Benchmark]
        public Task<CsvReader> Parse4MB()
        {
            return ParseCsvFile(Resources.FileSize.MB4);
        }

        [Benchmark]
        public async Task<CsvReader> FindLast_4MB()
        {
            const string ToFind = "596003.67";
            const int ColumnIndex = 8;

            int rowNumber = 0;
            int rowCount = 0;

            using (var stream = new StreamReader(Resources.GetStream(Resources.FileSize.MB4)))
            using (var reader = new CsvReader(stream, false))
            {
                while (await reader.ReadAsync())
                {
                    rowCount++;

                    if (ToFind == reader.GetField(ColumnIndex))
                    {
                        rowNumber = rowCount;
                    }
                }

                if (rowCount != rowNumber)
                {
                    throw new Exception("Failing benchmark because invalid conclusion was made");
                }

                return reader;
            }
        }

        private static async Task<CsvReader> ParseCsvFile(Resources.FileSize size)
        {
            using (var stream = new StreamReader(Resources.GetStream(size)))
            using (var reader = new CsvReader(stream, false))
            {
                int count = 0;

                while (await reader.ReadAsync())
                {
                    count++;
                    Debug.WriteLine(reader.ToString());
                }

                Debug.WriteLine($"Read {count} rows");

                return reader;
            }
        }
    }
}

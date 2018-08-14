using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class ChipotleCsvMemoryPoolBenchmark
    {
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

        [Benchmark]
        public async Task<Csv> FindLast_4MB()
        {
            Memory<byte> ToFind = Encoding.UTF8.GetBytes("596003.67").AsMemory();
            const int ColumnIndex = 8;

            int rowNumber = 0;
            int rowCount = 0;

            using (var stream = Resources.GetStream(Resources.FileSize.MB4))
            using (var reader = Csv.Parse(stream, new MemoryPoolRowProvider(stream)))
            {
                while (true)
                {
                    using (var row = await reader.GetNextAsync())
                    {

                        if (row == null)
                        {
                            break;
                        }

                        rowCount++;

                        if (row[ColumnIndex].SequenceEqual(ToFind.Span))
                        {
                            rowNumber = rowCount;
                        }
                    }
                }

                if (rowCount != rowNumber)
                {
                    throw new Exception($"Failing benchmark because invalid conclusion was made. Expected {rowCount} instead of {rowNumber}");
                }

                return reader;
            }
        }

        private static async Task<Csv> ParseCsvFile(Resources.FileSize size)
        {
            using (var stream = Resources.GetStream(size))
            using (var csv = Csv.Parse(stream, new MemoryPoolRowProvider(stream)))
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

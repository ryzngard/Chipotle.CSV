using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class ChipotleCsvStreamedMemoryPoolBenchmark
    {
        private static MemoryPoolRowProvider.Configuration GetConfiguration()
        {
            return new MemoryPoolRowProvider.Configuration()
            {
                RowParseMechanism = MemoryPoolRowProvider.RowParseMechanism.Streamed,
                LineParser = MemoryPoolRowProvider.LineParser.Streamed
            };
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
            using (var reader = Csv.Parse(stream, new MemoryPoolRowProvider(stream, config: GetConfiguration())))
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
            using (var csv = Csv.Parse(stream, new MemoryPoolRowProvider(stream, config: GetConfiguration())))
            {
                Debug.WriteLine($"Reading file, size = {stream.Length}");

                IRow<byte> row = null;
                int count = 0;
                while (true)
                {
                    var currentRow = await csv.GetNextAsync();

                    if (currentRow == null)
                    {
                        break;
                    }

                    row = currentRow;

                    Debug.WriteLine($"Row: ");
                    foreach (var segment in row)
                    {
                        Debug.WriteLine(Encoding.UTF8.GetString(segment.ToArray()));
                    }
                    Debug.WriteLine($"//Row");

                    count++;
                }

                Debug.WriteLine($"Read {count} rows");
                Debug.WriteLine($"Last Row: {row?.ToString() ?? "Null"}");

                return csv;
            }
        }
    }
}

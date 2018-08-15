using BenchmarkDotNet.Attributes;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Chipotle.CSV;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvBenchmarks
    {
        public enum ParsingMethod
        {
            CsvHelper,
            BytePipeline,
            MemoryPool,
            SMP
        };

        public static IEnumerable<ParsingMethod> ParsingMethods => Enum.GetValues(typeof(ParsingMethod)).Cast<ParsingMethod>();
        public static IEnumerable<Resources.FileSize> FileSizes => Enum.GetValues(typeof(Resources.FileSize)).Cast<Resources.FileSize>();

        public IEnumerable<(ParsingMethod, Resources.FileSize)> ParsingFilePairs()
        {

            return ParsingMethods
                .SelectMany(method => FileSizes.Select(size => (method, size)));
        }

        [Benchmark]
        [ArgumentsSource(nameof(ParsingFilePairs))]
        public Task<object> Parse((ParsingMethod, Resources.FileSize) pair)
        {
            return ParseCsvFile(pair.Item1, pair.Item2);
        }

        private async Task<object> ParseCsvFile(ParsingMethod method, Resources.FileSize fileSize)
        {
            var stream = Resources.GetStream(fileSize);

            IRowProvider rowProvider = null;

            switch (method)
            {
                case ParsingMethod.CsvHelper:
                    {
                        var streamReader = new StreamReader(stream);
                        var reader = new CsvReader(streamReader, false);

                        int count = 0;

                        while (await reader.ReadAsync())
                        {
                            count++;
                            Debug.WriteLine(reader.ToString());
                        }

                        Debug.WriteLine($"Read {count} rows");

                        return reader;
                    }

                case ParsingMethod.BytePipeline:
                    // This is the default 
                    break;

                case ParsingMethod.MemoryPool:
                    rowProvider = new MemoryPoolRowProvider(stream);
                    break;

                case ParsingMethod.SMP:
                    rowProvider = new MemoryPoolRowProvider(stream, config: new MemoryPoolRowProvider.Configuration()
                    {
                        RowParseMechanism = MemoryPoolRowProvider.RowParseMechanism.Streamed,
                        LineParser = MemoryPoolRowProvider.LineParser.Streamed
                    });

                    break;

                default:
                    throw new InvalidOperationException($"Parsing method is not handled! {method}");
            }

            {
                var csv = Csv.Parse(stream, rowProvider: rowProvider);

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

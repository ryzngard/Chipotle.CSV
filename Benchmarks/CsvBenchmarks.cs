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
using BenchmarkDotNet.Engines;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 5, targetCount: 5, id: "FastAndDirtyJob")]
    public class CsvBenchmarks
    {
        public enum ParsingMethod
        {
            CsvHelper,
            Pipeline,
            StreamReader,
            MemoryManaged
        };

        public IEnumerable<ParsingMethod> ParsingMethods()
        {
            return Enum.GetValues(typeof(ParsingMethod)).Cast<ParsingMethod>();
        }
        public IEnumerable<Resources.FileSize> FileSizes()
        {
            return Enum.GetValues(typeof(Resources.FileSize)).Cast<Resources.FileSize>();
        }

        public IEnumerable<object[]> ParsingFilePairs()
        {

            return ParsingMethods()
                .SelectMany(method => FileSizes().Select(size => new object[] { method, size }));
        }

        [Benchmark]
        [ArgumentsSource(nameof(ParsingFilePairs))]
        public async Task<object> Parse(ParsingMethod method, Resources.FileSize fileSize)
        {
            switch (method)
            {
                case ParsingMethod.CsvHelper:
                    {
                        var reader = (CsvReader)Open(method, fileSize);

                        int count = 0;

                        while (await reader.ReadAsync())
                        {
                            count++;
                            Debug.WriteLine(reader.ToString());
                        }

                        Debug.WriteLine($"Read {count} rows");

                        return reader;
                    }

                default:
                    {
                        var tokenizer = (ITokenizer)GetTokenizer(method, fileSize);
                        Debug.WriteLine($"Reading file, size = {fileSize}");

                        ISection row;
                        int count = 0;
                        while (true)
                        {
                            row = await tokenizer.GetNextAsync();

                            if (row == null)
                            {
                                break;
                            }

                            count++;
                        }

                        Debug.WriteLine($"Read {count} rows");
                        return tokenizer;
                    }
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(ParsingFilePairs))]
        public IDisposable Open(ParsingMethod method, Resources.FileSize fileSize)
        {
            return GetTokenizer(method, fileSize);
        }

        [Benchmark]
        [ArgumentsSource(nameof(ParsingMethods))]
        public async Task<object> Find_Last_4MB(ParsingMethod method)
        {
            const string ToFind = "596003.67";
            const int ColumnIndex = 8;

            int rowNumber = 0;
            int rowCount = 0;

            switch (method)
            {
                case ParsingMethod.CsvHelper:
                    {
                        var reader = (CsvReader)Open(method, Resources.FileSize.MB4);

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

                default:
                    {
                        var csv = (ITokenizer)GetTokenizer(method, Resources.FileSize.MB4);

                        while (true)
                        {
                            var row = await csv.GetNextAsync();
                            if (row == null)
                            {
                                break;
                            }

                            rowCount++;

                            if (ToFind.Equals(row[ColumnIndex].ToString()))
                            {
                                rowNumber = rowCount;
                            }
                        }

                        if (rowCount != rowNumber)
                        {
                            throw new Exception("Failing benchmark because invalid conclusion was made");
                        }
                        return csv;
                    }
            }
        }

        private IDisposable GetTokenizer(ParsingMethod method, Resources.FileSize fileSize)
        {
            switch (method)
            {
                case ParsingMethod.CsvHelper: return GetCsvHelperReader(fileSize);
                case ParsingMethod.Pipeline: return GetPipelineTokenizer(fileSize);
                case ParsingMethod.StreamReader: return GetStreamReaderTokenizer(fileSize);
                case ParsingMethod.MemoryManaged: return GetMemoryManagedTokenizer(fileSize);
                default: throw new InvalidOperationException();
            }
        }

        private CsvReader GetCsvHelperReader(Resources.FileSize fileSize)
        {
            var config = new CsvHelper.Configuration.Configuration()
            {
                Delimiter = Resources.GetSeperator(fileSize).ToString(),
                BadDataFound = (data) => Debug.WriteLine($"Bad Data Found: {data}")
            };

            var stream = new StreamReader(Resources.GetStream(fileSize));

            return new CsvReader(stream, config);
        }

        private ITokenizer GetPipelineTokenizer(Resources.FileSize fileSize)
        {
            return GetTokenizer<PipelineStreamTokenizer>(fileSize);
        }

        private ITokenizer GetStreamReaderTokenizer(Resources.FileSize fileSize)
        {
            return new StreamReaderTokenizer(new StreamReader(Resources.GetStream(fileSize)), disposeStream: true);
        }

        private ITokenizer GetMemoryManagedTokenizer(Resources.FileSize fileSize)
        {
            return GetTokenizer<MemoryManagedTokenizer>(fileSize);
        }

        private ITokenizer GetTokenizer<T>(Resources.FileSize fileSize)
            where T : class, ITokenizer
        {
            return (ITokenizer)Activator.CreateInstance(typeof(T), new object[] {
                Resources.GetStream(fileSize),
                new TokenizerSettings(
                    new byte[] { (byte)'\n', (byte)'\r' },
                    new byte[] { (byte)Resources.GetSeperator(fileSize) },
                    Encoding.UTF8,
                    true)
            });
        }
    }
}

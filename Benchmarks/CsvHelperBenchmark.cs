using BenchmarkDotNet.Attributes;
using CsvHelper;
using System.Diagnostics;
using System.IO;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvHelperBenchmark
    {
        [Benchmark]
        public CsvReader Parse2KB()
        {
            return ParseCsvFile(Resources.FileSize.KB2);
        }

        [Benchmark]
        public CsvReader Parse4KB()
        {
            return ParseCsvFile(Resources.FileSize.KB4);
        }

        [Benchmark]
        public CsvReader Parse8KB()
        {
            return ParseCsvFile(Resources.FileSize.KB8);
        }

        [Benchmark]
        public CsvReader Parse16KB()
        {
            return ParseCsvFile(Resources.FileSize.KB16);
        }

        [Benchmark]
        public CsvReader Parse32KB()
        {
            return ParseCsvFile(Resources.FileSize.KB32);
        }

        //[Benchmark]
        //public CsvReader Parse1MB()
        //{
        //    return ParseCsvFile("Import_User_Sample_en_Duplicated.csv");
        //}

        //[Benchmark]
        //public CsvReader Parse4MB()
        //{
        //    return ParseCsvFile("FL_insurance_sample.csv");
        //}

        private static CsvReader ParseCsvFile(Resources.FileSize size)
        {
            using (var stream = new StreamReader(Resources.GetStream(size)))
            using (var reader = new CsvReader(stream, false))
            {
                int count = 0;
                foreach (var record in reader.GetRecords<object>())
                {
                    count++;
                    Debug.WriteLine(record);
                }

                Debug.WriteLine($"Read {count} rows");

                return reader;
            }
        }
    }
}

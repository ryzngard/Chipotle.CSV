using BenchmarkDotNet.Attributes;
using CsvHelper;
using System.Diagnostics;
using System.IO;

namespace test
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class CsvHelperTests
    {
        private static StreamReader GetStreamReader(string name)
        {
            var assembly = typeof(ChipotleCsvTests).Assembly;
            return new StreamReader(assembly.GetManifestResourceStream($"test.{name}"));
        }

        [Benchmark]
        public CsvReader Parse2KB()
        {
            return ParseCsvFile("2KB.csv");
        }

        [Benchmark]
        public CsvReader Parse4KB()
        {
            return ParseCsvFile("4KB.csv");
        }

        [Benchmark]
        public CsvReader Parse8KB()
        {
            return ParseCsvFile("8KB.csv");
        }

        [Benchmark]
        public CsvReader Parse16KB()
        {
            return ParseCsvFile("16KB.csv");
        }

        [Benchmark]
        public CsvReader Parse32KB()
        {
            return ParseCsvFile("32KB.csv");
        }

        [Benchmark]
        public CsvReader Parse1MB()
        {
            return ParseCsvFile("Import_User_Sample_en_Duplicated.csv");
        }

        [Benchmark]
        public CsvReader Parse4MB()
        {
            return ParseCsvFile("FL_insurance_sample.csv");
        }

        private static CsvReader ParseCsvFile(string name)
        {
            using (var stream = GetStreamReader(name))
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

using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        #if DEBUG
        static void Main(string[] args)
        {
            var test = new CsvBenchmarks();

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Run {i}");
                test.Parse(CsvBenchmarks.ParsingMethod.MemoryManaged, Resources.FileSize.IMDB_Titles).Wait();
                //test.Find_Last_4MB(CsvBenchmarks.ParsingMethod.SMP).Wait();
            }
        }
        #else 
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        #endif
    }
}

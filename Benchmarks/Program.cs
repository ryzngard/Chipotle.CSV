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
                test.Parse((CsvBenchmarks.ParsingMethod.SMP, Resources.FileSize.MB4)).Wait();
            }
        }
        #else 
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        #endif
    }
}

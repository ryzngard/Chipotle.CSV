using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        #if DEBUG
        static void Main(string[] args)
        {
            var test = new ChipotleCsvStreamedMemoryPoolBenchmark();

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Run {i}");
                test.Parse4MB().Wait();
            }
        }
        #else 
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        #endif
    }
}

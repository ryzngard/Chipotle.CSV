using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        #if DEBUG
        static void Main(string[] args)
        {
            var test = new ChipotleCsvMemoryPoolBenchmark();

            test.FindLast_4MB().Wait();
        }
        #else 
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        #endif
    }
}

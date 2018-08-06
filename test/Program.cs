using System;
using BenchmarkDotNet.Running;

namespace test
{
    class Program
    {
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}

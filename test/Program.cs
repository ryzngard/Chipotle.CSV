using System;
using BenchmarkDotNet.Running;

namespace test
{
    class Program
    {
        #if DEBUG
        static void Main(string[] args)
        {
            var test = new CsvParsing();

            test.ParseCsv().Wait();
        }
        #else 
        static void Main(string[] args)
                => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        #endif
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benchmarks
{
    internal static class Resources
    {
        public enum FileSize
        {
            KB2,
            KB4,
            KB8,
            KB16,
            KB32,
            MB4
        }

        public static Stream GetStream(FileSize size)
        {
            string name = GetName(size);
            var assembly = typeof(Resources).Assembly;
            return assembly.GetManifestResourceStream($"Benchmarks.{name}");
        }

        private static string GetName(FileSize size)
        {
            switch (size)
            {
                case FileSize.KB2: return "2KB.csv";
                case FileSize.KB4: return "4KB.csv";
                case FileSize.KB8: return "8KB.csv";
                case FileSize.KB16: return "16KB.csv";
                case FileSize.KB32: return "32KB.csv";
                case FileSize.MB4: return "FL_insurance_sample.csv";
                default: throw new InvalidOperationException($"Unknown file for {size}");
            }
        }
    }
}

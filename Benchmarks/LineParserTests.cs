using BenchmarkDotNet.Attributes;
using Chipotle.CSV;
using CSharpx;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class LineParserTests
    {
        private const int LineSizeInBytes = 5000;
        private const int NumberOfSeperators = 100;
        private const byte LineBreak = (byte)'\n';
        private const byte Seperator = (byte)',';

        private ConcurrentQueue<IRow<byte>> _queue = new ConcurrentQueue<IRow<byte>>();

        [IterationSetup]
        public void Setup()
        {
            _queue.Clear();
        }

        [Benchmark]
        public int DefaultLineParser_Streamed_1k()
        {
            using (var stream = MakeStream(1000))
            {
                var defaultParser = new MemoryPoolLineParser(stream, _queue, Seperator, MemoryPoolRowProvider.RowParseMechanism.Streamed);

                defaultParser.Read();

                return _queue.Count;
            }
        }

        [Benchmark]
        public int DefaultLineParser_UpFront_1k()
        {
            using (var stream = MakeStream(1000))
            {
                var defaultParser = new MemoryPoolLineParser(stream, _queue, Seperator, MemoryPoolRowProvider.RowParseMechanism.Upfront);
                defaultParser.Read();

                return _queue.Count;
            }
        }

        [Benchmark]
        public int StreamLineParser_1k_Chunk10()
        {
            using (var stream = MakeStream(1000))
            {
                var defaultParser = new StreamLineParser(stream, chunkSizeInLines: 10);
                return defaultParser.Count();
            }
        }

        [Benchmark]
        public int StreamLineParser_1k_Chunk100()
        {
            using (var stream = MakeStream(1000))
            {
                var defaultParser = new StreamLineParser(stream, chunkSizeInLines: 100);
                return defaultParser.Count();
            }
        }

        [Benchmark]
        public int StreamLineParser_1k_Chunk1000()
        {
            using (var stream = MakeStream(1000))
            {
                var defaultParser = new StreamLineParser(stream, chunkSizeInLines: 1000);
                return defaultParser.Count();
            }
        }

        private Stream MakeStream(int lines)
        {
            var memoryStream = new MemoryStream(LineSizeInBytes * lines);

            Enumerable.Range(0, lines).ForEach(i =>
            {
                byte[] buffer = new byte[LineSizeInBytes];
                buffer[buffer.Length - 1] = LineBreak;

                int seperatorSpacing = LineSizeInBytes / NumberOfSeperators;
                for (int j = seperatorSpacing; j < (LineSizeInBytes-1); j = j + seperatorSpacing)
                {
                    buffer[j] = Seperator;
                }

                memoryStream.Write(buffer, 0, LineSizeInBytes);
            });

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}

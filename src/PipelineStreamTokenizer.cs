using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class PipelineStreamTokenizer : MultiThreadedStreamTokenizer
    {
        private Pipe _inputPipe = new Pipe();

        public PipelineStreamTokenizer(Stream stream, TokenizerSettings settings)
            : base(stream, settings)
        {
        }

        public PipelineStreamTokenizer(Stream stream)
            : base(stream)
        {
        }

        protected override async Task<ISection> GetNextAsyncInternal()
        {

            var segmentHints = new List<int>();
            var memorySegments = new List<ReadOnlyMemory<byte>>();
            while (true)
            {
                segmentHints.Clear();
                memorySegments.Clear();

                var read = await _inputPipe.Reader.ReadAsync();
                var buffer = read.Buffer;

                if (read.IsCanceled)
                {
                    break;
                }

                if (buffer.IsEmpty && read.IsCompleted)
                {
                    break;
                }

                try
                {
                    (SequenceStatus status, int size) = ParseSequence(buffer, segmentHints, memorySegments);

                    switch (status)
                    {
                        case SequenceStatus.DataAvailable:
                            // Always consume +1 from memory size, since it won't section delimiters
                            _inputPipe.Reader.AdvanceTo(buffer.GetPosition(size + 1, buffer.Start));
                            return new MemoryOwningSection(
                                new ArrayPoolMemoryOwner(memorySegments, size),
                                segmentHints,
                                Settings.Encoding);

                        case SequenceStatus.NeedsMoreData:
                            if (read.IsCompleted)
                            {
                                var section = new MemoryOwningSection(
                                    new ArrayPoolMemoryOwner(memorySegments, size),
                                    segmentHints,
                                    Settings.Encoding);

                                _inputPipe.Reader.Complete();
                                Completed = true;
                                return section;
                            }
                            else
                            {
                                _inputPipe.Reader.AdvanceTo(buffer.Start);
                            }
                            break;

                        case SequenceStatus.DataConsumed:
                            _inputPipe.Reader.AdvanceTo(buffer.GetPosition(size, buffer.Start));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            _inputPipe.Reader.Complete();
            Completed = true;
            return null;
        }

        protected override async Task Read(CancellationToken cancelToken)
        {
            Exception error = null;

            try
            {
                while (true)
                {
                    // Memory retrieved is often greater than the size hint.
                    var buffer = _inputPipe.Writer.GetMemory(1);
                    var bytes = await Stream.ReadAsync(buffer);
                    _inputPipe.Writer.Advance(bytes);

                    if (bytes == 0)
                    {
                        break;
                    }

                    var flush = await _inputPipe.Writer.FlushAsync();

                    if (flush.IsCompleted || flush.IsCanceled)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                _inputPipe.Writer.Complete(error);
            }
        }

        private enum SequenceStatus
        {
            NeedsMoreData,
            DataConsumed,
            DataAvailable
        }
        private (SequenceStatus, int) ParseSequence(ReadOnlySequence<byte> buffer, List<int> segmentHints, List<ReadOnlyMemory<byte>> memorySegments)
        {
            int memorySize = 0;

            foreach (var segment in buffer)
            {
                for (int i = 0; i < segment.Length; i++)
                {
                    var b = segment.Span[i];

                    if (IsSectionDelimiter(b))
                    {
                        if (i > 0)
                        {
                            memorySize += i;
                            memorySegments.Add(segment.Slice(0, i));
                        }

                        if (memorySize == 0)
                        {
                            return (SequenceStatus.DataConsumed, 1);
                        }

                        return (SequenceStatus.DataAvailable, memorySize);
                    }
                    else if (IsSegmentDelimiter(b))
                    {
                        segmentHints.Add(i + memorySize);
                    }
                }

                memorySegments.Add(segment);
                memorySize = checked(memorySize + segment.Length);
            }

            return (SequenceStatus.NeedsMoreData, memorySize);
        }
    }
}

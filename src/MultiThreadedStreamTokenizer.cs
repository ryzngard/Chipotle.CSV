using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chipotle.CSV;

public abstract class MultiThreadedStreamTokenizer : StreamTokenizer
{
    private CancellationTokenSource _readTokenSource = new();
    private Task _readTask;
    private int _streamReadCount = 0;
    private bool _needsToStart => Interlocked.CompareExchange(ref _streamReadCount, 1, 0) == 0;

    protected abstract Task Read(CancellationToken token);
    protected abstract Task<ISection> GetNextAsyncInternal();

    protected bool ReadFinished { get; private set; }

    public MultiThreadedStreamTokenizer(Stream stream)
        : base(stream)
    {
    }

    public MultiThreadedStreamTokenizer(Stream stream, TokenizerSettings? settings)
        : base(stream, settings)
    {
    }

    public override Task<ISection> GetNextAsync()
    {
        if (_needsToStart)
        {
            BeginRead();
        }

        if (Completed)
        {
            return Task.FromResult<ISection>(null);
        }

        return GetNextAsyncInternal();
    }

    private void BeginRead()
    {
        if (_readTask != null)
        {
            throw new InvalidOperationException();
        }

        _readTask = Task.Run(async () =>
        {
            await Read(_readTokenSource.Token);
            ReadFinished = true;
        }, _readTokenSource.Token);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _readTokenSource.Cancel();
            _readTask.Wait();

            _readTokenSource.Dispose();
            _readTask.Dispose();
        }

        base.Dispose(disposing);
    }
}

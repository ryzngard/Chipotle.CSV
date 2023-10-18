using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV;

internal static class Helpers
{
    internal static void FireAndForget<T>(this Task<T> task, bool continueOnCapturedContext = false)
    {
        task.ConfigureAwait(continueOnCapturedContext);
    }

    internal static void FireAndForget(this Task task, bool continueOnCapturedContext = false)
    {
        task.ConfigureAwait(continueOnCapturedContext);
    }
}

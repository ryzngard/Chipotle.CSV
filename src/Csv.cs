using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public class Csv : IDisposable
    {
        public IEnumerable<string> Headers { get; private set; }

        private readonly IRowProvider _rowProvider;
        public bool Completed => _rowProvider.Completed;

        protected Csv(IRowProvider rowProvider)
        {
            this._rowProvider = rowProvider;
        }

        public Task<IRow<byte>> GetNextAsync()
        {
            return _rowProvider.GetNextAsync();           
        }

        public static Csv Parse(Stream stream, IRowProvider rowProvider = null)
        {
            if (rowProvider == null)
            {
                rowProvider = new BytePipelineRowProvider(stream);
            }

            return new Csv(rowProvider);
        }

        public void Dispose()
        {
            this._rowProvider.Dispose();
        }
    }
}
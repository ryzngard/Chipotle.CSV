﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public interface IRowProvider : IDisposable
    {
        Task<Row<byte>> GetNextAsync();
        bool Completed { get; }
    }
}

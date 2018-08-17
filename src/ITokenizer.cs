using System;
using System.Threading.Tasks;

namespace Chipotle.CSV
{
    public interface ITokenizer : IDisposable
    {
        bool Completed { get; }
        Task<ISection> GetNextAsync();
    }
}

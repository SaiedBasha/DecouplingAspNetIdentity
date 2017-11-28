using System;
using System.Threading;
using System.Threading.Tasks;

namespace DecouplingAspNetIdentity.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit(bool resetAfterCommit);

        Task<int> CommitAsync(bool resetAfterCommit);

        Task<int> CommitAsync(CancellationToken cancellationToken, bool resetAfterCommit);

        void Undo();
    }
}
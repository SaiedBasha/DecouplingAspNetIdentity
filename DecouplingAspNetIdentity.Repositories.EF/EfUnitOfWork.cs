using System;
using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Repositories.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        public EfUnitOfWork(bool forceNewContext)
        {
            if (forceNewContext)
            {
                DataContextFactory.Clear();
            }
        }

        public int Commit(bool resetAfterCommit)
        {
            var result = DataContextFactory.GetDataContext().SaveChanges();
            if (resetAfterCommit)
            {
                DataContextFactory.Clear();
            }

            return result;
        }

        public Task<int> CommitAsync(bool resetAfterCommit)
        {
            var result = DataContextFactory.GetDataContext().SaveChangesAsync();
            if (resetAfterCommit)
            {
                DataContextFactory.Clear();
            }

            return result;
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken, bool resetAfterCommit)
        {
            var result = DataContextFactory.GetDataContext().SaveChangesAsync(cancellationToken);
            if (resetAfterCommit)
            {
                DataContextFactory.Clear();
            }

            return result;
        }

        public void Undo()
        {
            DataContextFactory.Clear();
        }

        // Flag: Has Dispose already been called?
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
                // Saves the changes to the underlying DbContext.
                DataContextFactory.GetDataContext().SaveChanges();
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }

        ~EfUnitOfWork()
        {
            Dispose(false);
        }
    }
}
using System;
using System.Linq;

namespace DecouplingAspNetIdentity.Infrastructure
{
    public interface IBusinessService : IDisposable
    {
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
    }

    public interface IBusinessService<TEntity, TId> : IBusinessService
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(TId id);
        void Save(TEntity entity);
        void Delete(TId id);
        void Delete(TEntity entity);
    }
}
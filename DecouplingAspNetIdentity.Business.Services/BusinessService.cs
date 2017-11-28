using System;
using System.Linq;
using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Business.Services
{
    public class BusinessService<TEntity,TId> : IBusinessService<TEntity, TId> 
        where TEntity : DomainEntity<TId>
        where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
    {
        public IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }
        protected readonly IRepository<TEntity, TId> Repository;
        private bool _disposed;

        public BusinessService(IUnitOfWorkFactory unitOfWorkFactory, IRepository<TEntity, TId> repository)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Repository = repository;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public TEntity GetById(TId id)
        {
            return Repository.GetById(id);
        }

        public void Save(TEntity entity)
        {
            using (UnitOfWorkFactory.Create(true))
            {
                Repository.Save(entity);
            }
        }

        public void Delete(TId id)
        {
            using (UnitOfWorkFactory.Create(true))
            {
                Repository.Delete(id);
            }
        }

        public void Delete(TEntity entity)
        {
            using (UnitOfWorkFactory.Create(true))
            {
                Repository.Delete(entity);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
            }

            _disposed = true;
        }
    }
}

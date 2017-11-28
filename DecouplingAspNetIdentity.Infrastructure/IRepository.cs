using System;
using System.Linq;
using System.Linq.Expressions;

namespace DecouplingAspNetIdentity.Infrastructure
{
    public interface IRepository<TEntity, TId> 
        where TEntity : DomainEntity<TId>
        where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
    {
        TEntity GetById(TId id, bool withTracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAll(bool withTracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        void Save(TEntity entity);
        void Delete(TEntity entity);
        void Delete(TId id);
    }
}

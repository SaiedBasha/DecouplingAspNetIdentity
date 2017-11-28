using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Repositories.EF.Repositories
{
    public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> 
        where TEntity : DomainEntity<TId>
        where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
    {
        public TEntity GetById(TId id, bool withTracking = true,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAll(withTracking, includeProperties)
                .SingleOrDefault(x =>  x.Id.Equals(id));
            //.SingleOrDefault(x => EqualityComparer<TId>.Default.Equals(x.Id, id));
        }

        public IQueryable<TEntity> GetAll(bool withTracking = true,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = DataContextFactory.GetDataContext().Set<TEntity>().AsQueryable();
            if (includeProperties != null && includeProperties.Length > 0)
            {
                items = includeProperties.Aggregate(items,
                    (current, includeProperty) => current.Include(includeProperty));
            }
            return withTracking ? items : items.AsNoTracking();
        }

        public void Save(TEntity entity)
        {
            DataContextFactory.GetDataContext().Entry(entity).State = EqualityComparer<TId>.Default.Equals(entity.Id, default(TId)) 
                ? EntityState.Added 
                : EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DataContextFactory.GetDataContext().Entry(entity).State = EntityState.Deleted;
            DataContextFactory.GetDataContext().Set<TEntity>().Remove(entity);
        }

        public void Delete(TId id)
        {
            var entity = GetById(id);
            Delete(entity);
        }
    }
}
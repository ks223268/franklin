using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Franklin.Data {

    public interface IRepository {

        FranklinDbContext DbContext { get; }

        void Create<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(object id) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null) where TEntity : class;
        TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "") where TEntity : class;
        void Save();
        void Update<TEntity>(TEntity entity) where TEntity : class;

    }
}
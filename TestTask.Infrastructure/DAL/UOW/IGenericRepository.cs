using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestTask.Common.DAL.UOW
{
    public interface IGenericRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        TEntity GetById(params object[] id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}

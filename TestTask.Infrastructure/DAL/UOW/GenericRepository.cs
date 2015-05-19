using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestTask.Common.DAL.UOW
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;


        /// <summary>Init repo</summary>        
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary> Return list of entities. (like LINQ 'Where') </summary>
        /// <param name="filter">Condition</param>
        /// <param name="includeProperties">Include entities</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            query = Filter(query, filter);
            query = Include(query, includeProperties);
            return query.ToList();
        }

        /// <summary> Return list of entities. (like LINQ 'FirstOrDefault') </summary>
        /// <param name="filter">Condition</param>
        /// <param name="includeProperties">Include entities</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            query = Filter(query, filter);
            query = Include(query, includeProperties);

            return query.FirstOrDefault();
        }

        public TEntity GetById(params object[] id)
        {
            if (id == null) return null;

            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(params object[] id)
        {
            if (id == null) return null;

            return await _dbSet.FindAsync(id);
        }

        public void Insert(TEntity entity)
        {
            if (entity == null) return;

            _dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            if (id == null) return;

            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) return;

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null) return;

            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        private IQueryable<TEntity> Filter(IQueryable<TEntity> data, Expression<Func<TEntity, bool>> filter)
        {
            return filter == null ? data : data.Where(filter);
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> data, string includeList)
        {
            foreach (var includeProperty in includeList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includeProperty);
            }

            return data;
        }
    }
}

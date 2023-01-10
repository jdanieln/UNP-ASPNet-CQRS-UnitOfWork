using Microsoft.EntityFrameworkCore;
using Nest;
using PermissionsWebApi.Data;
using System.Linq.Expressions;

namespace PermissionsWebApi.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected PermissionsWebApiContext _context;
        internal DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(PermissionsWebApiContext context, ILogger logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;

        }

        public virtual Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async void Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

    }
}

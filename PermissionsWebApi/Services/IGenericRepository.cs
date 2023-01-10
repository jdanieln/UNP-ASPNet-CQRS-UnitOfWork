using System.Linq.Expressions;

namespace PermissionsWebApi.Services
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        void Add(T entity);
        void Delete(int id);
        void Upsert(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}

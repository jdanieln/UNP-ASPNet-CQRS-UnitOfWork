using PermissionsWebApi.Models;
using System.Threading.Tasks;

namespace PermissionsWebApi.Application
{
    public interface IQueryHandler<M, C> where M : class where C : class
    {
        Task<IEnumerable<M>> GetAll();
        Task<Permission> GetOne(C query);
    }
}

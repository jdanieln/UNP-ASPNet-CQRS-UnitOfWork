using PermissionsWebApi.Configuration;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Application.QueryHandler
{
    public class QueryCommand
    {
        public int Id { get; set; }
    }
    public class PermissionQueryHandler: IQueryHandler<Permission, QueryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Permission>> GetAll()
        {
            return await _unitOfWork.Permission.All();
        }

        public async Task<Permission> GetOne(QueryCommand query)
        {
            return await _unitOfWork.Permission.GetById(query.Id);
        }
    }
}

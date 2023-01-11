
using Application.Commands;
using Domain;
using Domain.Configuration;
using Domain.Models;

namespace Application.QueryHandler
{
    public class PermissionQueryHandler: IQueryHandler<Permission, QueryByIdCommand>
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

        public async Task<Permission> GetOne(QueryByIdCommand query)
        {
            return await _unitOfWork.Permission.GetById(query.Id);
        }
    }
}

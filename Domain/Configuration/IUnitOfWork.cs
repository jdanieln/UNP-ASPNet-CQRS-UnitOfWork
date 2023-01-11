
using Domain.Services;

namespace Domain.Configuration
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permission { get; }
        IPermissionTypeRepository PermissionType { get; }

        void Commit();
        void Dispose();
    }
}

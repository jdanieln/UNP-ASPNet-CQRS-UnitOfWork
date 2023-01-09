using PermissionsWebApi.Services;

namespace PermissionsWebApi.Configuration
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permission { get; }
        IPermissionTypeRepository PermissionType { get; }

        Task CompleteAsync();
        void Dispose();
    }
}

using PermissionsWebApi.Services;

namespace PermissionsWebApi.Configuration
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permission { get; }
        Task CompleteAsync();
        void Dispose();
    }
}

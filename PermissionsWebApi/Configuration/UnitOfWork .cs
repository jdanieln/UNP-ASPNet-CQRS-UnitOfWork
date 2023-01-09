using Microsoft.EntityFrameworkCore;
using PermissionsWebApi.Services;

namespace PermissionsWebApi.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        public IPermissionRepository Permission { get; private set; }

        public UnitOfWork(DbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Permission = new PermissionRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

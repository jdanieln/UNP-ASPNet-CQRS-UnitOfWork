
using Nest;
using Domain.Services;
using Domain.Data;
using Microsoft.Extensions.Logging;

namespace Domain.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PermissionsWebApiContext _context;
        private readonly ILogger _logger;
        private readonly ElasticClient _elasticClient;

        public IPermissionRepository Permission { get; private set; }
        public IPermissionTypeRepository PermissionType { get; private set; }

        public UnitOfWork(PermissionsWebApiContext context, ElasticClient elasticClient, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            _elasticClient = elasticClient;

            Permission = new PermissionRepository(context, elasticClient, _logger);

            PermissionType = new PermissionTypeRepository(context, elasticClient, _logger);
        }


        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

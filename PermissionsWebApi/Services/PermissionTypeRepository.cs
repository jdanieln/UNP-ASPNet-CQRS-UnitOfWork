using Microsoft.EntityFrameworkCore;
using Nest;
using PermissionsWebApi.Data;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Services
{
    public class PermissionTypeRepository : GenericRepository<PermissionType>, IPermissionTypeRepository
    {
        private IElasticClient _elasticClient;
        public PermissionTypeRepository(PermissionsWebApiContext context, ElasticClient elasticClient, ILogger logger) : base(context, logger)
        {
            _elasticClient = elasticClient;
        }

        public override async Task<IEnumerable<PermissionType>> All()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(PermissionTypeRepository));
                return new List<PermissionType>();
            }
        }
        public override async Task<bool> Upsert(PermissionType entity)
        {
            try
            {
                var existingEntity = await _dbSet.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                {
                    await _elasticClient.IndexDocumentAsync(entity);
                    return await Add(entity);
                }


                existingEntity.Description = entity.Description;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(PermissionTypeRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await _dbSet.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                _dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(PermissionRepository));
                return false;
            }
        }
    }
    
    }


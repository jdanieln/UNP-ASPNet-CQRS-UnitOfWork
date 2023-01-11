using Domain.Data;
using Domain.Models;
using Nest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Services
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
        public override async void Upsert(PermissionType entity)
        {
            try
            {
                var existingEntity = await _dbSet.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                {
                    await _elasticClient.IndexDocumentAsync(entity);
                    Add(entity);
                }


                existingEntity.Description = entity.Description;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(PermissionTypeRepository));
                throw new Exception();
            }
        }

        public override async void Delete(int id)
        {
            try
            {
                var exist = await _dbSet.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) throw new Exception();

                _dbSet.Remove(exist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(PermissionRepository));
                throw new Exception();
            }
        }
    }
    
    }


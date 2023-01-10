﻿using Microsoft.EntityFrameworkCore;
using Nest;
using PermissionsWebApi.Data;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Services
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private IElasticClient _elasticClient;
        public PermissionRepository(PermissionsWebApiContext context, ElasticClient elasticClient, ILogger logger) : base(context, logger)
        {
            _elasticClient = elasticClient;
        }

        public override async Task<IEnumerable<Permission>> All()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(PermissionRepository));
                return new List<Permission>();
            }
        }
        public override async void Upsert(Permission entity)
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


                existingEntity.EmployeeForename = entity.EmployeeForename;
                existingEntity.EmployeeSurname = entity.EmployeeSurname;
                existingEntity.PermissionDate = entity.PermissionDate;
                existingEntity.PermissionTypeId = entity.PermissionTypeId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(PermissionRepository));
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

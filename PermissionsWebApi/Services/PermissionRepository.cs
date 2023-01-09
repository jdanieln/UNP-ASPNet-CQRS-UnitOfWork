using Microsoft.EntityFrameworkCore;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Services
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Permission>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(PermissionRepository));
                return new List<Permission>();
            }
        }
        public override async Task<bool> Upsert(Permission entity)
        {
            try
            {
                var existingEntity = await dbSet.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.EmployeeForename = entity.EmployeeForename;
                existingEntity.EmployeeSurname = entity.EmployeeSurname;
                existingEntity.PermissionDate = entity.PermissionDate;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(PermissionRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

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

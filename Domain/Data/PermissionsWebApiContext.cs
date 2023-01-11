using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Data
{
    public class PermissionsWebApiContext : DbContext
    {
        public PermissionsWebApiContext(DbContextOptions<PermissionsWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<Permission> Permission { get; set; } = default!;

        public DbSet<PermissionType> PermissionType { get; set; }

    }
}

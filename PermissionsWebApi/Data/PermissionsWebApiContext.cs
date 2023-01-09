using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Data
{
    public class PermissionsWebApiContext : DbContext
    {
        public PermissionsWebApiContext (DbContextOptions<PermissionsWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<PermissionsWebApi.Models.Permission> Permission { get; set; } = default!;

        public DbSet<PermissionsWebApi.Models.PermissionType> PermissionType { get; set; }
    }
}

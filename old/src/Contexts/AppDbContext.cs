using Microsoft.EntityFrameworkCore;
using Models;


namespace Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<PermissionRole> PermissionRole { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }
    }
}

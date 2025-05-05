using Abp.Zero.EntityFrameworkCore;
using VQM.Authorization.Roles;
using VQM.Authorization.Users;
using VQM.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace VQM.EntityFrameworkCore;

public class VQMDbContext : AbpZeroDbContext<Tenant, Role, User, VQMDbContext>
{
    /* Define a DbSet for each entity of the application */

    public VQMDbContext(DbContextOptions<VQMDbContext> options)
        : base(options)
    {
    }
}

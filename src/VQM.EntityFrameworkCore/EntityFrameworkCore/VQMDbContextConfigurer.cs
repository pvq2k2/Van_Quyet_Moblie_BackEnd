using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace VQM.EntityFrameworkCore;

public static class VQMDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<VQMDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<VQMDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}

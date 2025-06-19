using Domain.Entities;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // 1. Tạo danh sách Permission nếu chưa có
            var allPermissions = AppPermissions.All;
            foreach (var permissionName in allPermissions)
            {
                if (!await context.Permissions.AnyAsync(p => p.Name == permissionName))
                {
                    context.Permissions.Add(new Permission { Name = permissionName });
                }
            }
            await context.SaveChangesAsync();

            // 2. Tạo Role "User" trước vì User mặc định RoleId = 1
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                userRole = new Role { Name = "User" };
                context.Roles.Add(userRole);
                await context.SaveChangesAsync(); // Lưu để đảm bảo RoleId có giá trị
            }

            // 3. Tạo Role "Admin"
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role { Name = "Admin" };
                context.Roles.Add(adminRole);
                await context.SaveChangesAsync();
            }

            // 4. Gán toàn bộ permission cho Admin
            var permissionsInDb = await context.Permissions.ToListAsync();
            foreach (var permission in permissionsInDb)
            {
                bool exists = await context.RolePermissions
                    .AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id);
                if (!exists)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permission.Id
                    });
                }
            }
            await context.SaveChangesAsync();

            // 5. Tạo tài khoản admin
            string adminEmail = "admin@app.com";
            if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin123!");
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    RoleId = adminRole.Id,
                    Status = 1,
                    VerifiedAt = DateTime.UtcNow,
                    Password = hashedPassword
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}

using FashionShop.Data.Data;
using FashionShop.Data.Entities;
using BCrypt.Net;

namespace FashionShop.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Nếu DB đã có User, bỏ qua
            if (context.Users.Any())
            {
                return;
            }

            // Tạo Admin mặc định
            var adminUser = new User
            {
                FullName = "Administrator",
                Email = "admin@athea.vn",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}


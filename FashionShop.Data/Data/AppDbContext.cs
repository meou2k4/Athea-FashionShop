using Microsoft.EntityFrameworkCore;
using FashionShop.Data.Entities;

namespace FashionShop.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình kiểu decimal cho Product
            modelBuilder.Entity<Product>()
                .Property(p => p.BasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice)
                .HasColumnType("decimal(18,2)");

            // DisplayOrder trong Category phải là duy nhất (không trùng thứ tự)
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.DisplayOrder)
                .IsUnique();

            // Cascade delete: xóa Product sẽ xóa toàn bộ Variant của nó
            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .Property(pv => pv.Price)
                .HasColumnType("decimal(18,2)");

            // Cascade delete: xóa Product sẽ xóa toàn bộ Image của nó
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

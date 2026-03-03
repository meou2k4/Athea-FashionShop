using FashionShop.Data.Data;
using FashionShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FashionShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size)
                .ToListAsync();
        }

        public async Task<Product> GetProductWithFullDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);
        }
    }
}

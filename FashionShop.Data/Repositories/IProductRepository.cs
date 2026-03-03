using FashionShop.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FashionShop.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
        Task<Product> GetProductWithFullDetailsAsync(int id);
        Task<Product> GetProductBySlugAsync(string slug);

    }
}

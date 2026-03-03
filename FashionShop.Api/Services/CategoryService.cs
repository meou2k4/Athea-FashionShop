using FashionShop.Api.Models;
using FashionShop.Data.Entities;
using FashionShop.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShop.Api.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<(CategoryDto dto, string error)> CreateAsync(CreateCategoryDto request);
        Task<(bool success, string error)> UpdateAsync(int id, CreateCategoryDto request);
        Task<bool> DeleteAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.OrderBy(c => c.DisplayOrder).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                DisplayOrder = c.DisplayOrder
            });
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                DisplayOrder = category.DisplayOrder
            };
        }

        public async Task<(CategoryDto dto, string error)> CreateAsync(CreateCategoryDto request)
        {
            // Kiểm tra trùng DisplayOrder
            var existing = await _repository.FirstOrDefaultAsync(c => c.DisplayOrder == request.DisplayOrder);
            if (existing != null)
                return (null, $"Thứ tự hiển thị ({request.DisplayOrder}) đã được dùng bởi danh mục '{existing.Name}'.");

            // Kiểm tra trùng Slug
            var slugExist = await _repository.FirstOrDefaultAsync(c => c.Slug == request.Slug.ToLower().Trim());
            if (slugExist != null)
                return (null, $"Slug '{request.Slug}' đã tồn tại. Vui lòng đặt slug khác.");

            var category = new Category
            {
                Name = request.Name.Trim(),
                Slug = request.Slug.ToLower().Trim(),
                DisplayOrder = request.DisplayOrder
            };

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return (new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                DisplayOrder = category.DisplayOrder
            }, null);
        }

        public async Task<(bool success, string error)> UpdateAsync(int id, CreateCategoryDto request)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return (false, null);

            // Kiểm tra trùng DisplayOrder với danh mục khác
            var conflict = await _repository.FirstOrDefaultAsync(c => c.DisplayOrder == request.DisplayOrder && c.Id != id);
            if (conflict != null)
                return (false, $"Thứ tự hiển thị ({request.DisplayOrder}) đã được dùng bởi danh mục '{conflict.Name}'.");

            // Kiểm tra trùng Slug với danh mục khác
            var slugConflict = await _repository.FirstOrDefaultAsync(c => c.Slug == request.Slug.ToLower().Trim() && c.Id != id);
            if (slugConflict != null)
                return (false, $"Slug '{request.Slug}' đã tồn tại.");

            category.Name = request.Name.Trim();
            category.Slug = request.Slug.ToLower().Trim();
            category.DisplayOrder = request.DisplayOrder;

            _repository.Update(category);
            await _repository.SaveChangesAsync();
            return (true, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return false;

            _repository.Remove(category);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}

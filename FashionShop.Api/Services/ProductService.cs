using FashionShop.Api.Models;
using FashionShop.Data.Entities;
using FashionShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShop.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductVariantListDto>> GetVariantListAsync(int? categoryId, bool? isNew, bool? isOnSale);
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> GetBySlugAsync(string slug);
        Task<ProductDto> CreateAsync(CreateProductDto request);
        Task<bool> UpdateAsync(int id, CreateProductDto request);
        Task<bool> DeleteAsync(int id);
        Task<ProductVariantDto> AddVariantAsync(int productId, CreateProductVariantDto request);
        Task<bool> DeleteVariantAsync(int productId, int variantId);
        Task<bool> UpdateVariantAsync(int productId, int variantId, UpdateProductVariantDto request);
        Task<ProductImageDto> AddImageAsync(int productId, CreateProductImageDto request);
        Task<bool> DeleteImageAsync(int productId, int imageId);
        Task<bool> UpdateImageAsync(int productId, int imageId, CreateProductImageDto request);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<ProductVariant> _variantRepository;
        private readonly IRepository<ProductImage> _imageRepository;

        public ProductService(
            IProductRepository productRepository,
            IRepository<ProductVariant> variantRepository,
            IRepository<ProductImage> imageRepository)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _imageRepository = imageRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetProductsWithDetailsAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                CategorySlug = p.Category?.Slug,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                StorageInstructions = p.StorageInstructions,
                BasePrice = p.BasePrice,
                IsActive = p.IsActive,
                IsNew = p.IsNew,
                IsOnSale = p.IsOnSale,
                SalePrice = p.SalePrice,
                MainImageUrl = p.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? p.Images?.FirstOrDefault()?.ImageUrl
            });
        }

        public async Task<IEnumerable<ProductVariantListDto>> GetVariantListAsync(int? categoryId, bool? isNew, bool? isOnSale)
        {
            var products = await _productRepository.GetProductsWithDetailsAsync();

            // Apply filters and sorting
            if (categoryId.HasValue) products = products.Where(p => p.CategoryId == categoryId.Value);
            
            if (isNew == true) 
                products = products.Where(p => p.IsNew).OrderByDescending(p => p.CreatedAt);
                
            if (isOnSale == true) 
                products = products.Where(p => p.IsOnSale).OrderByDescending(p => p.BasePrice - (p.SalePrice ?? p.BasePrice));

            var result = new List<ProductVariantListDto>();
            foreach (var p in products.Where(p => p.IsActive))
            {
                // Group variants by color
                var colorGroups = p.Variants?
                    .Where(v => v.Color != null)
                    .GroupBy(v => v.ColorId)
                    .ToList() ?? new List<IGrouping<int, FashionShop.Data.Entities.ProductVariant>>();

                string statusValue = p.IsOnSale ? "sale" : (p.IsNew ? "mới" : null);

                if (!colorGroups.Any())
                {
                    // Product with no variants - show once with main image
                    result.Add(new ProductVariantListDto
                    {
                        ProductId = p.Id, ProductName = p.Name, ProductSlug = p.Slug,
                        CategoryId = p.CategoryId, CategoryName = p.Category?.Name,
                        BasePrice = p.BasePrice, SalePrice = p.SalePrice,
                        IsNew = p.IsNew, IsOnSale = p.IsOnSale,
                        MainImageUrl = p.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? p.Images?.FirstOrDefault()?.ImageUrl,
                        Status = statusValue
                    });
                }
                else
                {
                    foreach (var grp in colorGroups)
                    {
                        var color = grp.First().Color;
                        var mainImg = p.Images?.FirstOrDefault(i => i.ColorId == grp.Key && i.IsMain)
                                   ?? p.Images?.FirstOrDefault(i => i.ColorId == grp.Key);
                        result.Add(new ProductVariantListDto
                        {
                            ProductId = p.Id, ProductName = p.Name, ProductSlug = p.Slug,
                            CategoryId = p.CategoryId, CategoryName = p.Category?.Name,
                            BasePrice = p.BasePrice, SalePrice = p.SalePrice,
                            IsNew = p.IsNew, IsOnSale = p.IsOnSale,
                            ColorId = grp.Key, ColorName = color?.Name, ColorHex = color?.HexCode,
                            MainImageUrl = mainImg?.ImageUrl,
                            AllSizes = grp.Select(v => v.Size?.Name).Where(s => s != null).ToList(),
                            Status = statusValue
                        });
                    }
                }
            }
            return result;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetProductWithFullDetailsAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                StorageInstructions = product.StorageInstructions,
                BasePrice = product.BasePrice,
                IsActive = product.IsActive,
                IsNew = product.IsNew,
                IsOnSale = product.IsOnSale,
                SalePrice = product.SalePrice,
                MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? product.Images?.FirstOrDefault()?.ImageUrl,
                Images = product.Images?.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ColorId = i.ColorId,
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList() ?? new List<ProductImageDto>(),
                Variants = product.Variants?.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    ColorId = v.ColorId,
                    ColorName = v.Color?.Name,
                    ColorHex = v.Color?.HexCode,
                    SizeId = v.SizeId,
                    SizeName = v.Size?.Name,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariantDto>()
            };
        }

        public async Task<ProductDto> GetBySlugAsync(string slug)
        {
            var product = await _productRepository.GetProductBySlugAsync(slug);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                CategorySlug = product.Category?.Slug,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                StorageInstructions = product.StorageInstructions,
                BasePrice = product.BasePrice,
                IsActive = product.IsActive,
                IsNew = product.IsNew,
                IsOnSale = product.IsOnSale,
                SalePrice = product.SalePrice,
                MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? product.Images?.FirstOrDefault()?.ImageUrl,
                Images = product.Images?.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ColorId = i.ColorId,
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList() ?? new List<ProductImageDto>(),
                Variants = product.Variants?.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    ColorId = v.ColorId,
                    ColorName = v.Color?.Name,
                    ColorHex = v.Color?.HexCode,
                    SizeId = v.SizeId,
                    SizeName = v.Size?.Name,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariantDto>()
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto request)
        {
            // Kiểm tra SalePrice phải nhỏ hơn BasePrice
            if (request.IsOnSale && request.SalePrice.HasValue && request.SalePrice >= request.BasePrice)
                throw new Exception("Giá sale phải nhỏ hơn giá gốc (BasePrice).");

            // Kiểm tra trùng Slug
            var slugExist = await _productRepository.FirstOrDefaultAsync(p => p.Slug == request.Slug.ToLower().Trim());
            if (slugExist != null)
                throw new Exception($"Slug '{request.Slug}' đã tồn tại. Vui lòng đặt slug khác.");

            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name.Trim(),
                Slug = request.Slug.ToLower().Trim(),
                Description = request.Description?.Trim(),
                StorageInstructions = request.StorageInstructions?.Trim(),
                BasePrice = request.BasePrice,
                IsActive = request.IsActive,
                IsNew = request.IsNew,
                IsOnSale = request.IsOnSale,
                SalePrice = request.IsOnSale ? request.SalePrice : null,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return new ProductDto { Id = product.Id };
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto request)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            // Kiểm tra SalePrice phải nhỏ hơn BasePrice
            if (request.IsOnSale && request.SalePrice.HasValue && request.SalePrice >= request.BasePrice)
                throw new Exception("Giá sale phải nhỏ hơn giá gốc (BasePrice).");

            // Kiểm tra trùng Slug với sản phẩm khác
            var slugConflict = await _productRepository.FirstOrDefaultAsync(p => p.Slug == request.Slug.ToLower().Trim() && p.Id != id);
            if (slugConflict != null)
                throw new Exception($"Slug '{request.Slug}' đã tồn tại.");

            product.CategoryId = request.CategoryId;
            product.Name = request.Name.Trim();
            product.Slug = request.Slug.ToLower().Trim();
            product.Description = request.Description?.Trim();
            product.StorageInstructions = request.StorageInstructions?.Trim();
            product.BasePrice = request.BasePrice;
            product.IsActive = request.IsActive;
            product.IsNew = request.IsNew;
            product.IsOnSale = request.IsOnSale;
            product.SalePrice = request.IsOnSale ? request.SalePrice : null;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            _productRepository.Remove(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ProductVariantDto> AddVariantAsync(int productId, CreateProductVariantDto request)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new Exception("Không tìm thấy sản phẩm.");

            // Kiểm tra biến thể (Màu + Size) đã tồn tại chưa
            var variantExist = await _variantRepository.FirstOrDefaultAsync(
                v => v.ProductId == productId && v.ColorId == request.ColorId && v.SizeId == request.SizeId);
            if (variantExist != null)
                throw new Exception("Biến thể với Màu và Size này đã tồn tại cho sản phẩm này.");

            var variant = new ProductVariant
            {
                ProductId = productId,
                ColorId = request.ColorId,
                SizeId = request.SizeId,
                Price = request.Price,
                Stock = request.Stock
            };

            await _variantRepository.AddAsync(variant);
            await _variantRepository.SaveChangesAsync();

            return new ProductVariantDto { Id = variant.Id };
        }

        public async Task<bool> DeleteVariantAsync(int productId, int variantId)
        {
            var variant = await _variantRepository.FirstOrDefaultAsync(v => v.ProductId == productId && v.Id == variantId);
            if (variant == null) return false;

            _variantRepository.Remove(variant);
            await _variantRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateVariantAsync(int productId, int variantId, UpdateProductVariantDto request)
        {
            var variant = await _variantRepository.FirstOrDefaultAsync(v => v.ProductId == productId && v.Id == variantId);
            if (variant == null) return false;

            variant.Price = request.Price;
            variant.Stock = request.Stock;
            _variantRepository.Update(variant);
            await _variantRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ProductImageDto> AddImageAsync(int productId, CreateProductImageDto request)
        {
            var product = await _productRepository.GetProductWithFullDetailsAsync(productId);
            if (product == null) throw new Exception("Không tìm thấy sản phẩm");

            // Nếu là ảnh main, xóa main cũ cùng màu
            if (request.IsMain)
            {
                var samColorMains = product.Images
                    .Where(i => i.IsMain && i.ColorId == request.ColorId)
                    .ToList();
                foreach (var img in samColorMains)
                {
                    img.IsMain = false;
                    _imageRepository.Update(img);
                }
            }

            var image = new ProductImage
            {
                ProductId = productId,
                ColorId = request.ColorId,
                ImageUrl = request.ImageUrl,
                IsMain = request.IsMain
            };

            await _imageRepository.AddAsync(image);
            await _imageRepository.SaveChangesAsync();

            return new ProductImageDto { Id = image.Id };
        }

        public async Task<bool> DeleteImageAsync(int productId, int imageId)
        {
            var image = await _imageRepository.FirstOrDefaultAsync(i => i.ProductId == productId && i.Id == imageId);
            if (image == null) return false;

            _imageRepository.Remove(image);
            await _imageRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateImageAsync(int productId, int imageId, CreateProductImageDto request)
        {
            var image = await _imageRepository.FirstOrDefaultAsync(i => i.ProductId == productId && i.Id == imageId);
            if (image == null) return false;

            // Nếu đặt làm main, xóa main cũ cùng màu
            if (request.IsMain)
            {
                var product = await _productRepository.GetProductWithFullDetailsAsync(productId);
                foreach (var img in product.Images.Where(i => i.IsMain && i.ColorId == request.ColorId && i.Id != imageId))
                {
                    img.IsMain = false;
                    _imageRepository.Update(img);
                }
            }

            image.ImageUrl = request.ImageUrl;
            image.ColorId = request.ColorId;
            image.IsMain = request.IsMain;
            _imageRepository.Update(image);
            await _imageRepository.SaveChangesAsync();
            return true;
        }
    }
}

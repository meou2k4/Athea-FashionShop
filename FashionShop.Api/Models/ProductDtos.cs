using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Api.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string StorageInstructions { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsOnSale { get; set; }
        public decimal? SalePrice { get; set; }

        public string MainImageUrl { get; set; }

        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Danh mục không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId không hợp lệ.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Slug không được để trống.")]
        [StringLength(250, ErrorMessage = "Slug không được vượt quá 250 ký tự.")]
        public string Slug { get; set; }

        [StringLength(5000, ErrorMessage = "Mô tả không được vượt quá 5000 ký tự.")]
        public string Description { get; set; }

        [StringLength(5000, ErrorMessage = "Hướng dẫn bảo quản không được vượt quá 5000 ký tự.")]
        public string StorageInstructions { get; set; }

        [Required(ErrorMessage = "Giá bán không được để trống.")]
        [Range(100, 1_000_000_000, ErrorMessage = "Giá bán phải từ 100 đến 1,000,000,000 VND.")]
        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsNew { get; set; } = false;
        public bool IsOnSale { get; set; } = false;

        [Range(0, 1_000_000_000, ErrorMessage = "Giá sale không hợp lệ.")]
        public decimal? SalePrice { get; set; }
    }

    public class ProductVariantDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
    }

    public class CreateProductVariantDto
    {
        [Required(ErrorMessage = "Vui lòng chọn màu sắc.")]
        [Range(1, int.MaxValue, ErrorMessage = "ColorId không hợp lệ.")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn kích thước.")]
        [Range(1, int.MaxValue, ErrorMessage = "SizeId không hợp lệ.")]
        public int SizeId { get; set; }

        [Range(0, 1_000_000_000, ErrorMessage = "Giá biến thể phải từ 0 đến 1,000,000,000 VND.")]
        public decimal? Price { get; set; }

        [Range(0, 99999, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 99,999.")]
        public int Stock { get; set; }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? ColorId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }

    public class CreateProductImageDto
    {
        [Required(ErrorMessage = "URL hình ảnh không được để trống.")]
        [StringLength(1000, ErrorMessage = "URL quá dài.")]
        public string ImageUrl { get; set; }

        public int? ColorId { get; set; }
        public bool IsMain { get; set; }
    }

    public class UpdateProductVariantDto
    {
        [Range(0, 1_000_000_000, ErrorMessage = "Giá biến thể phải từ 0 đến 1,000,000,000 VND.")]
        public decimal? Price { get; set; }

        [Range(0, 99999, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 99,999.")]
        public int Stock { get; set; }
    }

    /// <summary>Mỗi item = 1 màu của 1 sản phẩm (dùng cho trang danh sách khách hàng)</summary>
    public class ProductVariantListDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public bool IsNew { get; set; }
        public bool IsOnSale { get; set; }

        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public string MainImageUrl { get; set; }
        public string Status { get; set; } // new, sale, null

        public List<string> AllSizes { get; set; } = new();
    }
}

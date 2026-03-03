using System;
using System.Collections.Generic;

namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng sản phẩm (Product) - trung tâm của hệ thống.
    /// Lưu thông tin chung của 1 mặt hàng thời trang.
    /// </summary>
    public class Product
    {
        /// <summary>Khóa chính - ID duy nhất của sản phẩm.</summary>
        public int Id { get; set; }

        /// <summary>
        /// Khóa ngoại trỏ đến bảng Category.
        /// Xác định sản phẩm này thuộc danh mục nào (VD: Váy Đầm, Áo nữ...).
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>Tên sản phẩm. Ví dụ: "Áo sơ mi trắng cổ V".</summary>
        public string Name { get; set; }

        /// <summary>
        /// Đường dẫn URL thân thiện (slug). Dùng cho SEO và route.
        /// Ví dụ: "ao-so-mi-trang-co-v".
        /// </summary>
        public string Slug { get; set; }

        /// <summary>Mô tả chi tiết về sản phẩm: chất liệu, kiểu dáng, cách phối đồ...</summary>
        public string Description { get; set; }

        /// <summary>Hướng dẫn bảo quản sản phẩm.</summary>
        public string StorageInstructions { get; set; }

        /// <summary>
        /// Giá cả bán (giá tham khảo gốc) của sản phẩm.
        /// Giá thực tế từng biến thể (màu + size) được lưu ở bảng ProductVariant.
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// Trạng thái hiển thị trên website.
        /// True = đang bán / hiển thị, False = ẩn / ngừng bán.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Đánh dấu đây là sản phẩm mới (xuất hiện trong group "SẢN PHẨM MỚI" trên menu).
        /// True = sản phẩm mới, False = bình thường.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Đánh dấu sản phẩm đang được giảm giá (xuất hiện trong group "SALE OFF").
        /// True = đang sale, False = không sale.
        /// </summary>
        public bool IsOnSale { get; set; }

        /// <summary>
        /// Giá sau khi giảm (giá sale). Chỉ dùng khi IsOnSale = true.
        /// Null nếu không có khuyến mại.
        /// </summary>
        public decimal? SalePrice { get; set; }

        /// <summary>Thời điểm sản phẩm được tạo. Lưu theo giờ UTC.</summary>
        public DateTime CreatedAt { get; set; }

        // --- Navigation Properties ---

        /// <summary>Trỏ đến đối tượng danh mục chứa sản phẩm này.</summary>
        public Category Category { get; set; }

        /// <summary>Danh sách các biến thể (màu + size) của sản phẩm.</summary>
        public ICollection<ProductVariant> Variants { get; set; }

        /// <summary>Danh sách hình ảnh của sản phẩm (tối đa 6 ảnh: 1 chính + 5 phụ).</summary>
        public ICollection<ProductImage> Images { get; set; }
    }
}

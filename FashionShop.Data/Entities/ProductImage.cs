namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng hình ảnh sản phẩm (ProductImage).
    /// Mỗi sản phẩm có tối đa 6 ảnh: 1 ảnh chính + 5 ảnh chi tiết.
    /// </summary>
    public class ProductImage
    {
        /// <summary>Khóa chính - ID duy nhất của ảnh.</summary>
        public int Id { get; set; }

        /// <summary>Khóa ngoại trỏ đến bảng Product - sản phẩm mà ảnh này thuộc về.</summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Khóa ngoại tùy chọn - ảnh này đặc trưng cho màu nào.
        /// Null nếu ảnh áp dụng chung cho tất cả màu.
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// Đường dẫn (URL) đến file ảnh.
        /// Ví dụ: "/uploads/products/ao-so-mi-1.jpg" hoặc link CDN.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Đánh dấu đây có phải là ảnh chính (thumbnail) của sản phẩm không.
        /// True = đây là ảnh đại diện (chỉ được có 1 ảnh IsMain = true mỗi sản phẩm).
        /// False = ảnh chi tiết phụ.
        /// </summary>
        public bool IsMain { get; set; }

        // --- Navigation Properties ---

        /// <summary>Trỏ đến sản phẩm cha.</summary>
        public Product Product { get; set; }
    }
}

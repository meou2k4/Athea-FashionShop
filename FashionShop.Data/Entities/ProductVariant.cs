namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng biến thể sản phẩm (ProductVariant).
    /// Mỗi biến thể = 1 tổ hợp (Màu + Size) cụ thể của sản phẩm.
    /// Ví dụ: Áo sơ mi trắng - Size M - Màu Trắng là 1 biến thể.
    /// </summary>
    public class ProductVariant
    {
        /// <summary>Khóa chính - ID duy nhất của biến thể.</summary>
        public int Id { get; set; }

        /// <summary>Khóa ngoại trỏ đến bảng Product - sản phẩm mà biến thể này thuộc về.</summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Khóa ngoại trỏ đến bảng Color - màu sắc của biến thể này.
        /// Null nếu sản phẩm không phân loại theo màu.
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// Khóa ngoại trỏ đến bảng Size - kích thước của biến thể này.
        /// Null nếu sản phẩm không phân loại theo size.
        /// </summary>
        public int SizeId { get; set; }

        /// <summary>
        /// Giá bán của riêng biến thể này (có thể khác giá có bán của sản phẩm).
        /// Ví dụ: Size XL có thể bán giá cao hơn S.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho hiện tại của biến thể này.
        /// 0 = hết hàng.
        /// </summary>
        public int Stock { get; set; }

        // --- Navigation Properties ---

        /// <summary>Trỏ đến sản phẩm cha.</summary>
        public Product Product { get; set; }

        /// <summary>Trỏ đến đối tượng Màu sắc.</summary>
        public Color Color { get; set; }

        /// <summary>Trỏ đến đối tượng Kích thước.</summary>
        public Size Size { get; set; }
    }
}

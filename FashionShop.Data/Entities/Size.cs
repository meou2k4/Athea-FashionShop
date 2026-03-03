using System.Collections.Generic;

namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng kích thước (Size).
    /// Dùng để phân loại biến thể sản phẩm theo size.
    /// </summary>
    public class Size
    {
        /// <summary>Khóa chính - ID duy nhất của kích thước.</summary>
        public int Id { get; set; }

        /// <summary>Tên kích thước. Ví dụ: "S", "M", "L", "XL", "XXL".</summary>
        public string Name { get; set; }

        // --- Navigation Properties ---

        /// <summary>Danh sách các biến thể sản phẩm có kích thước này.</summary>
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}

using System.Collections.Generic;

namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng màu sắc (Color).
    /// Dùng để phân loại biến thể sản phẩm theo màu.
    /// </summary>
    public class Color
    {
        /// <summary>Khóa chính - ID duy nhất của màu sắc.</summary>
        public int Id { get; set; }

        /// <summary>Tên màu sắc. Ví dụ: "Đỏ đô", "Xanh navy".</summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã màu HEX để hiển thị trực tiếp trên giao diện web.
        /// Ví dụ: "#FF5733" (màu đỏ), "#1A1A2E" (xanh đậm).
        /// </summary>
        public string HexCode { get; set; }

        // --- Navigation Properties ---

        /// <summary>Danh sách các biến thể sản phẩm có màu này.</summary>
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}

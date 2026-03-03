using System.Collections.Generic;

namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng danh mục sản phẩm (Category) - cấu trúc phẳng (flat), không phân cấp cha con.
    /// Ví dụ danh mục: "Váy Đầm", "Áo Vest nữ", "Áo Dài", "Túi Xách Tay"...
    /// Các nhóm như "Sản Phẩm Mới" hay "Sale Off" là thuộc tính lọc trên bảng Product, không phải Category.
    /// </summary>
    public class Category
    {
        /// <summary>Khóa chính - ID duy nhất của danh mục.</summary>
        public int Id { get; set; }

        /// <summary>Tên hiển thị của danh mục. Ví dụ: "Váy Đầm", "Áo phông", "Túi Xách Tay".</summary>
        public string Name { get; set; }

        /// <summary>
        /// Đường dẫn URL thân thiện (slug). Dùng cho SEO và route.
        /// Ví dụ: "vay-dam", "ao-phong", "tui-xach-tay".
        /// </summary>
        public string Slug { get; set; }

        /// <summary>Thứ tự hiển thị trên menu (số nhỏ hơn hiển thị trước).</summary>
        public int DisplayOrder { get; set; }

        // --- Navigation Properties ---

        /// <summary>Danh sách sản phẩm thuộc danh mục này.</summary>
        public ICollection<Product> Products { get; set; }
    }
}

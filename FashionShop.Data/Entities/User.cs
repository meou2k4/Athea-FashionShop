using System;

namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng tài khoản người dùng / quản trị viên (User).
    /// Trong hệ thống này chỉ có vai trò Admin mới được thêm/sửa/xóa sản phẩm.
    /// </summary>
    public class User
    {
        /// <summary>Khóa chính - ID duy nhất của tài khoản.</summary>
        public int Id { get; set; }

        /// <summary>
        /// Họ tên đầy đủ của người dùng.
        /// Ví dụ: "Nguyễn Văn Admin".
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Địa chỉ email dùng để đăng nhập vào hệ thống quản trị.
        /// Phải là duy nhất trong toàn hệ thống.
        /// Ví dụ: "admin@fashionshop.vn".
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Mật khẩu đã được mã hóa bằng BCrypt.
        /// KHÔNG lưu mật khẩu thô. Không bao giờ hiển thị giá trị này ra API.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Vai trò (quyền hạn) của tài khoản trong hệ thống.
        /// Hiện tại chỉ có 2 giá trị: "Admin" (quản trị) hoặc "User" (người dùng thông thường).
        /// </summary>
        public string Role { get; set; }

        /// <summary>Thời điểm tài khoản được tạo. Lưu theo giờ UTC.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Thời điểm tài khoản được cập nhật lần cuối. Null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedAt { get; set; }
    }
}

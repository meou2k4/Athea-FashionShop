namespace FashionShop.Data.Entities
{
    /// <summary>
    /// Bảng cài đặt hệ thống (Setting).
    /// Lưu các cấu hình dạng key-value để admin thay đổi mà không cần sửa code.
    /// Ví dụ: Số hotline, link Zalo, link Messenger.
    /// </summary>
    public class Setting
    {
        /// <summary>Khóa chính - ID duy nhất của cài đặt.</summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên khóa (định danh duy nhất) của cài đặt.
        /// Ví dụ: "Hotline", "ZaloUrl", "MessengerUrl".
        /// Code sử dụng key này để đọc giá trị.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Giá trị thực tế của cài đặt.
        /// Ví dụ: "0901234567", "https://zalo.me/xxx", "https://m.me/xxx".
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Ghi chú mô tả cài đặt này dùng để làm gì.
        /// Ví dụ: "Số điện thoại hotline tư vấn khách hàng".
        /// </summary>
        public string Description { get; set; }
    }
}

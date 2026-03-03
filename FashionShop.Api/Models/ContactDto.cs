using System.ComponentModel.DataAnnotations;

namespace FashionShop.Api.Models
{
    public class ContactDto
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn.")]
        public string Message { get; set; }
    }
}

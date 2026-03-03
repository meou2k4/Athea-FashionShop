using System.ComponentModel.DataAnnotations;

namespace FashionShop.Api.Models
{
    public class ColorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
    }

    public class CreateColorDto
    {
        [Required(ErrorMessage = "Tên màu không được để trống.")]
        [StringLength(50, ErrorMessage = "Tên màu không được vượt quá 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã màu HEX không được để trống.")]
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
            ErrorMessage = "Mã HEX không hợp lệ. Định dạng đúng: #RRGGBB hoặc #RGB. Ví dụ: #FF5733.")]
        public string HexCode { get; set; }
    }

    public class SizeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateSizeDto
    {
        [Required(ErrorMessage = "Tên kích thước không được để trống.")]
        [StringLength(20, ErrorMessage = "Tên kích thước không được vượt quá 20 ký tự.")]
        public string Name { get; set; }
    }
}

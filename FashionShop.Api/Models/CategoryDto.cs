using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Api.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Slug không được để trống.")]
        [StringLength(150, ErrorMessage = "Slug không được vượt quá 150 ký tự.")]
        [RegularExpression(@"^[a-z0-9\u00C0-\u024F\s\-]+$", ErrorMessage = "Slug chỉ được chứa chữ thường, số, dấu gạch ngang.")]
        public string Slug { get; set; }

        [Range(1, 9999, ErrorMessage = "Thứ tự hiển thị phải từ 1 đến 9999.")]
        public int DisplayOrder { get; set; }
    }
}

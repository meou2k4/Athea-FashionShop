using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Upload ảnh sản phẩm. File được lưu vào wwwroot/images/products/
        /// </summary>
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Không có file được gửi." });

            // Chỉ chấp nhận ảnh
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
                return BadRequest(new { message = $"Chỉ chấp nhận định dạng: {string.Join(", ", allowedExtensions)}" });

            // Giới hạn 10MB
            if (file.Length > 10 * 1024 * 1024)
                return BadRequest(new { message = "File quá lớn (tối đa 10MB)." });

            // Tạo thư mục nếu chưa có
            var uploadFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images", "products");
            Directory.CreateDirectory(uploadFolder);

            // Tên file duy nhất
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/images/products/{fileName}";

            return Ok(new
            {
                fileName,
                url = relativePath,
                message = "Upload thành công!"
            });
        }
    }
}

using FashionShop.Api.Models;
using FashionShop.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendContact([FromBody] ContactDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _emailService.SendContactEmailAsync(
                    request.Name,
                    request.Email,
                    request.Phone,
                    request.Message);

                return Ok(new { message = "Gửi thông tin liên hệ thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi gửi email.", error = ex.Message });
            }
        }
    }
}

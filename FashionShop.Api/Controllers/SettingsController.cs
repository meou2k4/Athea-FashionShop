using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionShop.Data.Data;
using FashionShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShop.Api.Controllers
{
    public class SettingDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/settings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SettingDto>>> GetSettings()
        {
            return await _context.Settings
                .Select(s => new SettingDto
                {
                    Key = s.Key,
                    Value = s.Value,
                    Description = s.Description
                })
                .ToListAsync();
        }

        // POST/PUT: api/settings
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSettings([FromBody] List<SettingDto> requests)
        {
            foreach (var req in requests)
            {
                var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == req.Key);
                if (setting == null)
                {
                    _context.Settings.Add(new Setting
                    {
                        Key = req.Key,
                        Value = req.Value,
                        Description = req.Description
                    });
                }
                else
                {
                    setting.Value = req.Value;
                    if (!string.IsNullOrEmpty(req.Description))
                    {
                        setting.Description = req.Description;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Cập nhật cấu hình thành công!" });
        }
    }
}

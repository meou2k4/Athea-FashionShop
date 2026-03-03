using FashionShop.Api.Models;
using FashionShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FashionShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public ColorController(IPropertyService propertyService) => _propertyService = propertyService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetColors()
        {
            return Ok(await _propertyService.GetColorsAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostColor([FromBody] CreateColorDto request)
        {
            var (dto, error) = await _propertyService.CreateColorAsync(request);
            if (error != null) return BadRequest(new { message = error });
            return Ok(dto);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public SizeController(IPropertyService propertyService) => _propertyService = propertyService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SizeDto>>> GetSizes()
        {
            return Ok(await _propertyService.GetSizesAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostSize([FromBody] CreateSizeDto request)
        {
            var (dto, error) = await _propertyService.CreateSizeAsync(request);
            if (error != null) return BadRequest(new { message = error });
            return Ok(dto);
        }
    }
}

using FashionShop.Api.Models;
using FashionShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FashionShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("variants-list")]
        public async Task<ActionResult<IEnumerable<ProductVariantListDto>>> GetVariantList(
            [FromQuery] int? categoryId,
            [FromQuery] bool? isNew,
            [FromQuery] bool? isOnSale)
        {
            var items = await _productService.GetVariantListAsync(categoryId, isNew, isOnSale);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<ProductDto>> GetProductBySlug(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);
            if (product == null) return NotFound(new { message = "Không tìm thấy sản phẩm." });
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> PostProduct([FromBody] CreateProductDto request)
        {
            var product = await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] CreateProductDto request)
        {
            var result = await _productService.UpdateAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // --- VARIANTS --- //
        [HttpPost("{id}/variants")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductVariantDto>> PostProductVariant(int id, [FromBody] CreateProductVariantDto request)
        {
            try
            {
                var variant = await _productService.AddVariantAsync(id, request);
                return Ok(new { Message = "Thêm biến thể thành công!", VariantId = variant.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/variants/{variantId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductVariant(int id, int variantId)
        {
            var result = await _productService.DeleteVariantAsync(id, variantId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/variants/{variantId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProductVariant(int id, int variantId, [FromBody] UpdateProductVariantDto request)
        {
            var result = await _productService.UpdateVariantAsync(id, variantId, request);
            if (!result) return NotFound();
            return NoContent();
        }

        // --- IMAGES --- //
        [HttpPost("{id}/images")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductImageDto>> PostProductImage(int id, [FromBody] CreateProductImageDto request)
        {
            try
            {
                var image = await _productService.AddImageAsync(id, request);
                return Ok(new { Message = "Thêm hình ảnh thành công!", ImageId = image.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/images/{imageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductImage(int id, int imageId)
        {
            var result = await _productService.DeleteImageAsync(id, imageId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/images/{imageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProductImage(int id, int imageId, [FromBody] CreateProductImageDto request)
        {
            var result = await _productService.UpdateImageAsync(id, imageId, request);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

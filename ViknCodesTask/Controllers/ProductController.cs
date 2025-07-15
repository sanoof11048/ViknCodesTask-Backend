using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViknCodesTask.Common;
using ViknCodesTask.DTOs;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.Interface;

namespace ViknCodesTask.Controllers
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO dto)
        {
            try
            {
                if (Request.Form.TryGetValue("Variants", out var variantsJson))
                {
                    dto.Variants = JsonConvert.DeserializeObject<List<ProductVariantDTO>>(variantsJson!);
                }

                var result = await _productService.CreateProductAsync(dto);

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Guid>(500, "Unexpected server error", default, ex.Message));
            }
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductListAsync();
            return Ok(products);
        }

        [HttpPut("stock/update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockDTO dto)
        {
            var result = await _productService.UpdateStockAsync(dto);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDTO dto)
        {
            try
            {
                if (Request.Form.TryGetValue("Variants", out var variantsJson))
                {
                    dto.Variants = JsonConvert.DeserializeObject<List<ProductVariantDTO>>(variantsJson!);
                }

                var result = await _productService.UpdateProductAsync(dto);

                if (!result.Success)
                    return StatusCode(result.StatusCode, result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Guid>(500, "Error updating product", default, ex.Message));
            }
        }


    }
}


//[HttpGet]
//public IActionResult GetUserId()
//{
//    var userId = HttpContext.Items["UserId"]?.ToString();
//    return Ok(userId);
//}

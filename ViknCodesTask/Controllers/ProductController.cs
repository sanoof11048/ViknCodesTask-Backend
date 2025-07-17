using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViknCodesTask.Common;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.Interface;

namespace ViknCodesTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        //[Authorize]
        //[HttpPost("create")]
        //public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO dto)
        //{
        //    try
        //    {
        //        if (Request.Form.TryGetValue("Variants", out var variantsJson))
        //        {
        //            dto.Variants = JsonConvert.DeserializeObject<List<ProductVariantDTO>>(variantsJson!);
        //        }

        //        if (Request.Form.TryGetValue("combinations", out var comboJson))
        //        {
        //            dto.Combinations = JsonConvert.DeserializeObject<List<ProductCombinationStockDTO>>(comboJson!);
        //        }

        //        var result = await _productService.CreateProductAsync(dto);

        //        if (!result.Success)
        //        {
        //            return StatusCode(result.StatusCode, result);
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponse<Guid>(500, "Unexpected server error", default, ex.Message));
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // In a real app, get user ID from auth context
                var userId = User?.Identity?.Name ?? "system";

                var product = await _productService.CreateProductAsync(productDto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new { message = "An error occurred while creating the product" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("stock/add")]
        public async Task<IActionResult> AddStock([FromBody] StockMovementDTO stockDto)
        {
            // In a real app, get user ID from auth context
            var userId = Guid.Parse(User?.Identity?.Name ?? "00000000-0000-0000-0000-000000000001");

            var result = await _productService.AddStockAsync(stockDto);
            return StatusCode(result, result);
        }

        [HttpPost("stock/remove")]
        public async Task<IActionResult> RemoveStock([FromBody] StockMovementDTO stockDto)
        {
            // In a real app, get user ID from auth context
            var userId = Guid.Parse(User?.Identity?.Name ?? "00000000-0000-0000-0000-000000000001");

            var result = await _productService.RemoveStockAsync(stockDto);
            return StatusCode(result, result);
        }

        //        [Authorize]
        //        [HttpGet("all")]
        //        public async Task<IActionResult> GetProducts()
        //        {
        //            var products = await _productService.GetProductListAsync();
        //            return Ok(products);
        //        }


        //        [Authorize]
        //        [HttpGet("{id}")]
        //        public async Task<IActionResult> GetProductById(Guid id)
        //        {
        //            var result = await _productService.GetProductByIdAsync(id);
        //            if (!result.Success)
        //                return StatusCode(result.StatusCode, result);

        //            return Ok(result);
        //        }
    }
}


//[HttpGet]
//public IActionResult GetUserId()
//{
//    var userId = HttpContext.Items["UserId"]?.ToString();
//    return Ok(userId);
//}

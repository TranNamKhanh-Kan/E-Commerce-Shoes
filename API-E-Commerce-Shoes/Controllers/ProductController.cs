using BAL.IService;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_E_Commerce_Shoes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("get-all-product")]
        public IActionResult GetAllProduct()
        {
            return Ok(_service.GetAllProduct());
        }

        [HttpGet("get-product-by-id/{id:guid}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _service.GetProductById(id);
            if (product == null) return NotFound("Product not found");
            return Ok(product);
        }

        [HttpGet("search")]
        public IActionResult Search(string? keyword,string? type,string? status)
        {
            return Ok(_service.SearchProducts(keyword, type, status));
        }

        [HttpPost("create-product")]
        [Authorize(Roles = "1,2")]
        public IActionResult CreateProduct([FromBody] ProductRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Invalid product data");

            var product = _service.CreateProduct(request);
            return Ok(product);
        }

        [HttpPut("update-product/{id:guid}")]
        [Authorize(Roles = "1,2")]
        public IActionResult UpdateProduct(Guid id, [FromBody] ProductRequest request)
        {
            var product = _service.UpdateProductById(id, request);
            if (product == null) return NotFound("Product not found");
            return Ok(product);
        }

        [HttpDelete("delete-product/{id:guid}")]
        [Authorize(Roles = "1")]
        public IActionResult DeleteProduct(Guid id)
        {
            var result = _service.DeleteProduct(id);
            if (!result) return NotFound("Product not found");
            return Ok(new { message = "Product deleted or marked inactive" });
        }
    }
}

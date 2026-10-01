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
        public IActionResult Search(string? keyword, string? type, string? status)
        {
            return Ok(_service.SearchProducts(keyword, type, status));
        }

        /// <summary>
        /// Tạo sản phẩm. Gửi multipart/form-data với field Image (file).
        /// ImageUrl trong DB sẽ là SecureUrl từ Cloudinary.
        /// </summary>
        [HttpPost("create-product")]
        [Authorize(Roles = "1,2")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request, IFormFile image)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Invalid product data");

            try
            {
                var product = await _service.CreateProduct(request, image);
                return Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm. Nếu gửi Image mới thì upload Cloudinary và cập nhật ImageUrl;
        /// nếu không gửi Image thì giữ ImageUrl cũ.
        /// </summary>
        [HttpPut("update-product/{id:guid}")]
        [Authorize(Roles = "1,2")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductRequest request, IFormFile? image)
        {
            try
            {
                var product = await _service.UpdateProductById(id, request, image);
                if (product == null) return NotFound("Product not found");
                return Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
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

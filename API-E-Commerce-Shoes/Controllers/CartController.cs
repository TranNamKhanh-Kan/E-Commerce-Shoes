using BAL.IService;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_E_Commerce_Shoes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("get-cart")]
        [Authorize]
        public IActionResult GetCart([FromQuery] Guid userId)
        {
            try
            {
                return Ok(_service.GetCartByUserId(userId));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-to-cart")]
        [Authorize]
        public IActionResult AddToCart([FromBody] AddToCartDTO request)
        {
            try
            {
                return Ok(_service.AddToCart(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-item")]
        [Authorize]
        public IActionResult UpdateCartItem([FromBody] UpdateCartItemDTO request)
        {
            try
            {
                return Ok(_service.UpdateCartItem(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove-item")]
        [Authorize]
        public IActionResult RemoveCartItem([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            var result = _service.RemoveCartItem(userId, productId);
            if (!result) return NotFound("Cart item not found");
            return Ok(new { message = "Item removed" });
        }

        [HttpDelete("clear-cart")]
        [Authorize]
        public IActionResult ClearCart([FromQuery] Guid userId)
        {
            var result = _service.ClearCart(userId);
            if (!result) return NotFound("Cart not found");
            return Ok(new { message = "Cart cleared" });
        }

        [HttpPost("checkout")]
        [Authorize]
        public IActionResult Checkout([FromBody] CheckoutDTO request)
        {
            try
            {
                return Ok(_service.Checkout(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

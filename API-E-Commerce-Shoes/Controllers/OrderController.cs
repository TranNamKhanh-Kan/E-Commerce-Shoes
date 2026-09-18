using BAL.IService;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_E_Commerce_Shoes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("get-all-order")]
        [Authorize(Roles = "1,2")]
        public IActionResult GetAllOrder()
        {
            return Ok(_service.GetAllOrder());
        }

        [HttpGet("get-order-by-id/{id:guid}")]
        [Authorize]
        public IActionResult GetOrderById(Guid id)
        {
            var order = _service.GetOrderById(id);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpGet("get-order-by-user-id")]
        [Authorize]
        public IActionResult GetOrderByUserId([FromQuery] Guid id)
        {
            return Ok(_service.GetOrderByUserId(id));
        }

        [HttpPost("create-order")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] OrderCreateDTO request)
        {
            try
            {
                var order = _service.CreateOrder(request);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-order/{id:guid}")]
        [Authorize(Roles = "1,2")]
        public IActionResult UpdateOrder(Guid id, [FromBody] OrderUpdateDTO request)
        {
            try
            {
                var order = _service.UpdateOrder(id, request);
                if (order == null) return NotFound("Order not found");
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("cancel-order/{id:guid}")]
        [Authorize]
        public IActionResult CancelOrder(Guid id)
        {
            try
            {
                var result = _service.CancelOrder(id);
                if (!result) return NotFound("Order not found");
                return Ok(new { message = "Order cancelled" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

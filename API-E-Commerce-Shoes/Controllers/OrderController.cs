using BAL.IService;
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

        [HttpGet("get-order-by-user-id")]
        [Authorize]
        public IActionResult GetOrderByUserId(Guid id)
        {
            return Ok(_service.GetOrderByUserId(id));
        }
    }
}

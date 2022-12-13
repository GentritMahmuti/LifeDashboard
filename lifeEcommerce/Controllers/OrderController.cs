using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpPost("ProcessOrder")]
        public async Task<IActionResult> ProcessOrder(List<string> orderIds, string status)
        {
            await _orderService.ProcessOrder(orderIds, status);

            return Ok($"Now selected orders are in new status: {status}");
        }
    }
}

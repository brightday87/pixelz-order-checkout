using Microsoft.AspNetCore.Mvc;
using Pixelz.OrderCheckout.Api.Bussiness.Interface;

namespace Pixelz.OrderCheckout.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Tìm kiếm danh sách đơn hàng của khách hàng theo tên (draft only)
        /// </summary>
        [HttpGet("{customerId}/search")]
        public async Task<IActionResult> SearchOrders(Guid customerId, [FromQuery] string? orderName)
        {
            var result = await _orderService.GetOrdersAsync(customerId, orderName);
            return Ok(result);
        }

        /// <summary>
        /// Submit nhiều đơn hàng song song
        /// </summary>
        [HttpPost("{customerId}/submit")]
        public async Task<IActionResult> SubmitOrders(Guid customerId, [FromBody] IEnumerable<Guid> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
                return BadRequest("Order list cannot be empty.");

            var result = await _orderService.SubmitOrdersAsync(customerId, orderIds);
            return Ok(result);
        }

        /// <summary>
        /// Submit 1 đơn hàng
        /// </summary>
        [HttpPost("{customerId}/submit/{orderId}")]
        public async Task<IActionResult> SubmitOrder(Guid customerId, Guid orderId)
        {
            var result = await _orderService.SubmitOrderAsync(customerId, orderId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

using ECommerceOrdersAPI.Models;
using ECommerceOrdersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrdersAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetOrders();
            
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrdersById(int id)
        {
            var order = await _orderService.GetOrderById(id);

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] AddOrderDto dto)
        {
            var id = await _orderService.CreateOrder(dto);

            return CreatedAtAction(nameof(GetOrdersById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditOrder([FromRoute] int id, [FromBody] EditOrderDto dto)
        {
            await _orderService.EditOrder(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
               
            return NoContent();
        }
    }
}

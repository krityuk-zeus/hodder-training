using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Create a new order
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(GetAll), new { id = order.Id }, order);
        }

        // Get all orders
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // Get orders by PizzaId
        [HttpGet("pizza/{pizzaId}")]
        public async Task<ActionResult<List<Order>>> GetByPizza(int pizzaId)
        {
            var orders = await _orderService.GetByPizzaIdAsync(pizzaId);
            return Ok(orders);
        }
    }
}

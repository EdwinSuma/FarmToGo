using Microsoft.AspNetCore.Mvc;
using FTG.Repository.Repository;
using Farmers.DataModel;
using System.Threading.Tasks;

namespace Farmers.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;

        // Constructor with dependency injection of the order repository
        public OrderController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (order == null)
            {
                return BadRequest("Order data is required.");
            }

            // Directly call the AddOrderAsync method without assignment
            await _orderRepo.AddOrderAsync(order);

            // After adding the order, fetch it again to return the newly created order
            var createdOrder = await _orderRepo.GetOrderByIdAsync(order.OrderId);

            // If the order is created successfully, return the created order
            if (createdOrder == null)
            {
                return BadRequest("Error creating the order.");
            }

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }

        // PUT: api/Order/{id}  
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest("Order ID mismatch.");
            }

            var existingOrder = await _orderRepo.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            await _orderRepo.UpdateOrderAsync(order);
            return NoContent(); // Success response without content
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            return Ok(order);
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            await _orderRepo.DeleteOrderAsync(id);
            return NoContent(); // Success response without content
        }

    }
}

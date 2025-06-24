using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly HotPotDbContext _context;

        public OrdersController(IOrderService orderService, HotPotDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var order = await _orderService.PlaceOrder(userId, createOrderDTO);
                return CreatedAtAction(nameof(GetOrderDetails), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to place order: {ex.Message}");
            }
        }

        [HttpGet("user")]
        [Authorize(Roles = "User,Admin,Restaurant")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var orders = await _orderService.GetUserOrders(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve user orders: {ex.Message}");
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                    return NotFound("Order not found");

                order.Status = dto.Status;
                await _context.SaveChangesAsync();

                return Ok("Order status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update order status: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var success = await _orderService.DeleteOrder(id, userId);

                if (!success)
                    return NotFound("Order not found or you don't have permission to delete it");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete order: {ex.Message}");
            }
        }

        [HttpGet("restaurant/orders")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> GetRestaurantOrdersByLoggedInUser()
        {
            try
            {
                // Get logged-in user ID from claims
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Get the restaurant associated with this user
                var restaurant = await _context.Restaurants
                    .FirstOrDefaultAsync(r => r.UserId == userId);

                if (restaurant == null)
                    return NotFound("No restaurant associated with this user.");

                // Use restaurant.Id internally
                var orders = await _orderService.GetRestaurantOrders(restaurant.Id, userId);
                return Ok(orders);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch restaurant orders: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin,Restaurant")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            try
            {
                var order = await _orderService.GetOrderDetails(id);
                if (order == null)
                    return NotFound("Order not found");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch order details: {ex.Message}");
            }
        }
    }
}

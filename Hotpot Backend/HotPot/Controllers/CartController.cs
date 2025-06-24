using HotPot.DTOs;
using HotPot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var cart = await _cartService.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving cart: {ex.Message}");
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _cartService.AddToCart(userId, dto);
                return Ok("Item added to cart.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding item to cart: {ex.Message}");
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _cartService.UpdateCartItem(userId, dto.MenuItemId, dto.Quantity);
                return Ok("Cart item updated.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating cart item: {ex.Message}");
            }
        }

        [HttpDelete("remove/{itemId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _cartService.RemoveFromCart(userId, itemId);
                return Ok("Item removed from cart.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error removing item: {ex.Message}");
            }
        }

        [HttpDelete("clear")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _cartService.ClearCart(userId);
                return Ok("Cart cleared.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error clearing cart: {ex.Message}");
            }
        }
    }
}

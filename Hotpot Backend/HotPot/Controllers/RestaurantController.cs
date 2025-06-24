using System.Security.Claims;
using HotPot.DTOs;
using HotPot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var restaurants = await _restaurantService.GetAllRestaurants();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch restaurants: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            try
            {
                var restaurant = await _restaurantService.GetRestaurantById(id);
                if (restaurant == null)
                    return NotFound("Restaurant not found");
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch restaurant: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> AddRestaurant(CreateRestaurantDTO restaurantDTO)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var addedRestaurant = await _restaurantService.AddRestaurant(restaurantDTO, userId);
                return CreatedAtAction(nameof(GetRestaurantById), new { id = addedRestaurant.Id }, addedRestaurant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add restaurant: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantDTO restaurantDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var updated = await _restaurantService.UpdateRestaurant(id, restaurantDTO, currentUserId);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Restaurant not found");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, "You are not authorized to update this restaurant.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update restaurant: {ex.Message}");
            }
        }




        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            try
            {
                var success = await _restaurantService.DeleteRestaurant(id);
                if (!success)
                    return NotFound("Restaurant not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete restaurant: {ex.Message}");
            }
        }
    }
}

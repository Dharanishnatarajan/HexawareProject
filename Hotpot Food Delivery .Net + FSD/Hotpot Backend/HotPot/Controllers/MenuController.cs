using HotPot.DTOs;
using HotPot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _menuService.GetAllMenuItems();
            return Ok(menuItems);
        }

        [HttpGet("restaurant/{restaurantId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuItemsByRestaurant(int restaurantId)
        {
            var menuItems = await _menuService.GetMenuItemsByRestaurant(restaurantId);
            return Ok(menuItems);
        }

        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuItemsByCategory(int categoryId)
        {
            var menuItems = await _menuService.GetMenuItemsByCategory(categoryId);
            return Ok(menuItems);
        }

        [HttpPost]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> AddMenuItem(CreateMenuItemDTO menuItemDTO)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var addedMenuItem = await _menuService.AddMenuItem(menuItemDTO, userId);
                return Ok(addedMenuItem);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding menu item: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> UpdateMenuItem(int id, CreateMenuItemDTO menuItemDTO)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedMenuItem = await _menuService.UpdateMenuItem(id, menuItemDTO, userId);
                return Ok(updatedMenuItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating menu item: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var success = await _menuService.DeleteMenuItem(id, userId);
                if (!success)
                    return NotFound("Menu item not found or unauthorized.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting menu item: {ex.Message}");
            }
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMenuCategories()
        {
            var categories = await _menuService.GetAllMenuCategories();
            return Ok(categories);
        }

        [HttpPost("categories")]
        [Authorize(Roles = "Restaurant,Admin")]
        public async Task<IActionResult> AddMenuCategory(CreateMenuCategoryDTO menuCategoryDTO)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var addedCategory = await _menuService.AddMenuCategory(menuCategoryDTO, userId);
                return Ok(addedCategory);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding category: {ex.Message}");
            }
        }
    }
}

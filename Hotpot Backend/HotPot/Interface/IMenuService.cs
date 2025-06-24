using HotPot.DTOs;

namespace HotPot.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuItemDTO>> GetAllMenuItems();
        Task<MenuItemDTO> GetMenuItemById(int id);
        Task<List<MenuItemDTO>> GetMenuItemsByRestaurant(int restaurantId);
        Task<List<MenuItemDTO>> GetMenuItemsByCategory(int categoryId);

        // Authorization required for restaurant owner or admin
        Task<MenuItemDTO> AddMenuItem(CreateMenuItemDTO menuItemDTO, int currentUserId);
        Task<MenuItemDTO> UpdateMenuItem(int id, CreateMenuItemDTO menuItemDTO, int currentUserId);
        Task<bool> DeleteMenuItem(int id, int currentUserId);

        // Global categories
        Task<List<MenuCategoryDTO>> GetAllMenuCategories();

        // Removed restaurantId-based categories since categories are now global
        // Task<List<MenuCategoryDTO>> GetMenuCategoriesByRestaurant(int restaurantId); // REMOVE

        Task<MenuCategoryDTO> AddMenuCategory(CreateMenuCategoryDTO menuCategoryDTO, int currentUserId); // no ownership check needed now
    }
}

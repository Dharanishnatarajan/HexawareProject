using AutoMapper;
using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Services
{
    public class MenuService : IMenuService
    {
        private readonly HotPotDbContext _context;
        private readonly IMapper _mapper;

        public MenuService(HotPotDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuItemDTO>> GetAllMenuItems()
        {
            var menuItems = await _context.MenuItems
                .Include(mi => mi.MenuCategory)
                .Include(mi => mi.Restaurant)
                .ToListAsync();

            return _mapper.Map<List<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO> GetMenuItemById(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(mi => mi.MenuCategory)
                .Include(mi => mi.Restaurant)
                .FirstOrDefaultAsync(mi => mi.Id == id);

            return menuItem == null ? null : _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<List<MenuItemDTO>> GetMenuItemsByRestaurant(int restaurantId)
        {
            var menuItems = await _context.MenuItems
                .Include(mi => mi.MenuCategory)
                .Include(mi => mi.Restaurant)
                .Where(mi => mi.RestaurantId == restaurantId)
                .ToListAsync();

            return _mapper.Map<List<MenuItemDTO>>(menuItems);
        }

        public async Task<List<MenuItemDTO>> GetMenuItemsByCategory(int categoryId)
        {
            var menuItems = await _context.MenuItems
                .Include(mi => mi.MenuCategory)
                .Include(mi => mi.Restaurant)
                .Where(mi => mi.MenuCategoryId == categoryId)
                .ToListAsync();

            return _mapper.Map<List<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO> AddMenuItem(CreateMenuItemDTO dto, int currentUserId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == dto.RestaurantId);
            if (restaurant == null || restaurant.UserId != currentUserId)
                throw new UnauthorizedAccessException("Unauthorized to add items to this restaurant.");

            var menuItem = _mapper.Map<MenuItem>(dto);
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            var created = await _context.MenuItems
                .Include(x => x.Restaurant)
                .Include(x => x.MenuCategory)
                .FirstOrDefaultAsync(x => x.Id == menuItem.Id);

            return _mapper.Map<MenuItemDTO>(created);
        }

        public async Task<MenuItemDTO> UpdateMenuItem(int id, CreateMenuItemDTO dto, int currentUserId)
        {
            var menuItem = await _context.MenuItems
                .Include(x => x.Restaurant)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (menuItem == null || menuItem.Restaurant.UserId != currentUserId)
                throw new UnauthorizedAccessException("Unauthorized to update this item.");

            _mapper.Map(dto, menuItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<bool> DeleteMenuItem(int id, int currentUserId)
        {
            var menuItem = await _context.MenuItems
                .Include(x => x.Restaurant)
                .IgnoreQueryFilters() // include soft-deleted records
                .FirstOrDefaultAsync(x => x.Id == id);

            if (menuItem == null || menuItem.IsDeleted || menuItem.Restaurant.UserId != currentUserId)
                return false;

            menuItem.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<MenuCategoryDTO>> GetAllMenuCategories()
        {
            var categories = await _context.MenuCategories.ToListAsync();
            return _mapper.Map<List<MenuCategoryDTO>>(categories);
        }

        public async Task<MenuCategoryDTO> AddMenuCategory(CreateMenuCategoryDTO dto, int currentUserId)
        {
            // Global category: no restaurant ownership check needed
            var category = _mapper.Map<MenuCategory>(dto);
            _context.MenuCategories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<MenuCategoryDTO>(category);
        }
    }
}

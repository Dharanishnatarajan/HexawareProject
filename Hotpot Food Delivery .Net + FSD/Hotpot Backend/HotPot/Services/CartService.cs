using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Services
{
    public class CartService : ICartService
    {
        private readonly HotPotDbContext _context;

        public CartService(HotPotDbContext context)
        {
            _context = context;
        }

        public async Task AddToCart(int userId, AddToCartDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            var menuItem = await _context.MenuItems
                .Include(mi => mi.Restaurant)
                .FirstOrDefaultAsync(mi => mi.Id == dto.MenuItemId);

            if (menuItem == null)
                throw new KeyNotFoundException("Menu item not found.");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }
            else
            {
                var existingRestaurantId = cart.CartItems
                    .Select(ci => ci.MenuItem.RestaurantId)
                    .Distinct()
                    .FirstOrDefault();

                if (existingRestaurantId != 0 && existingRestaurantId != menuItem.RestaurantId)
                    throw new InvalidOperationException("You can only add items from one restaurant at a time.");
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.MenuItemId == dto.MenuItemId);
            if (existingItem != null)
                existingItem.Quantity += dto.Quantity;
            else
                cart.CartItems.Add(new CartItem { MenuItemId = dto.MenuItemId, Quantity = dto.Quantity });

            await _context.SaveChangesAsync();
        }


        public async Task UpdateCartItem(int userId, int menuItemId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(ci => ci.MenuItemId == menuItemId);
                if (item != null)
                {
                    item.Quantity = quantity;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Item not found in cart.");
                }
            }
        }


        public async Task<CartDTO> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return new CartDTO
            {
                Items = cart?.CartItems.Select(ci => new CartItemDTO
                {
                    MenuItemId = ci.MenuItemId,
                    MenuItemName = ci.MenuItem.Name,
                    Quantity = ci.Quantity,
                    Price = ci.MenuItem.Price
                }).ToList() ?? new List<CartItemDTO>()
            };
        }

        public async Task RemoveFromCart(int userId, int menuItemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(ci => ci.MenuItemId == menuItemId);
                if (item != null)
                    _context.CartItems.Remove(item);

                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
        }
    }
}

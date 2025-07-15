using AutoMapper;
using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Services
{
    public class OrderService : IOrderService
    {
        private readonly HotPotDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(HotPotDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDTO> PlaceOrder(int userId, CreateOrderDTO createOrderDTO)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty. Please add items before placing an order.");

            decimal total = 0;
            var orderItems = new List<OrderItem>();

            foreach (var ci in cart.CartItems)
            {
                total += ((decimal)(ci.MenuItem.DiscountPrice ?? ci.MenuItem.Price)) * ci.Quantity;

                orderItems.Add(new OrderItem
                {
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    Price = ci.MenuItem.Price
                });
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = total,
                Address = createOrderDTO.Address,
                PhoneNumber = createOrderDTO.PhoneNumber,
                Status = "Pending",
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems); 
            await _context.SaveChangesAsync();

            return await GetOrderDetails(order.Id);
        }



        public async Task<List<OrderDTO>> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                           .ThenInclude(mi => mi.Restaurant) // Include restaurant

                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<bool> DeleteOrder(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.IsDeleted)
                return false;

            if (order.UserId != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role != "Admin")
                    return false;
            }

            order.IsDeleted = true; // ✅ Soft delete
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<OrderDTO>> GetRestaurantOrders(int restaurantId, int currentUserId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant == null || restaurant.UserId != currentUserId)
                throw new UnauthorizedAccessException("Unauthorized to view orders of this restaurant.");

            var orders = await _context.OrderItems
                .Where(oi => oi.MenuItem.RestaurantId == restaurantId)
                .Select(oi => oi.Order)
                .Distinct()
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Restaurant) // Include restaurant
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.MenuCategory) // Include category
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return _mapper.Map<OrderDTO>(order);
        }

    }
}
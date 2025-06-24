using AutoMapper;
using HotPot.Data;
using HotPot.DTOs;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly HotPotDbContext _context;
        private readonly IMapper _mapper;

        public RestaurantService(HotPotDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDTO>> GetAllRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task<RestaurantDTO> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            return _mapper.Map<RestaurantDTO>(restaurant);
        }

        public async Task<RestaurantDTO> AddRestaurant(CreateRestaurantDTO restaurantDTO, int userId)
        {
            // Check if user already has a restaurant
            var existingRestaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            if (existingRestaurant != null)
                throw new InvalidOperationException("User already has a restaurant.");

            var restaurant = _mapper.Map<Restaurant>(restaurantDTO);
            restaurant.UserId = userId;

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return _mapper.Map<RestaurantDTO>(restaurant);
        }


        public async Task<RestaurantDTO> UpdateRestaurant(int id, UpdateRestaurantDTO restaurantDTO, int currentUserId)
        {
            var restaurant = await _context.Restaurants.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            // Check if current user is either the restaurant owner or an admin
            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null)
                throw new UnauthorizedAccessException();

            bool isAdmin = user.Role == "Admin";
            bool isOwner = restaurant.UserId == currentUserId;

            if (!isAdmin && !isOwner)
                throw new UnauthorizedAccessException();

            // Map only updatable fields from DTO
            _mapper.Map(restaurantDTO, restaurant);
            await _context.SaveChangesAsync();

            return _mapper.Map<RestaurantDTO>(restaurant);
        }




        public async Task<bool> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return false;

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
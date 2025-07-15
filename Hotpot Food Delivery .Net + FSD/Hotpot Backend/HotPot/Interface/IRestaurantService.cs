using HotPot.DTOs;

namespace HotPot.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDTO>> GetAllRestaurants();
        Task<RestaurantDTO> GetRestaurantById(int id);
        Task<RestaurantDTO> AddRestaurant(CreateRestaurantDTO restaurantDTO, int userId);

        // ✅ Updated to use UpdateRestaurantDTO instead of RestaurantDTO
        Task<RestaurantDTO> UpdateRestaurant(int id, UpdateRestaurantDTO restaurantDTO, int currentUserId);

        Task<bool> DeleteRestaurant(int id);
    }
}

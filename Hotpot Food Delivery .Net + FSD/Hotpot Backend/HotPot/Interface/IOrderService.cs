using HotPot.DTOs;

namespace HotPot.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> PlaceOrder(int userId, CreateOrderDTO createOrderDTO);
        Task<List<OrderDTO>> GetUserOrders(int userId);
        Task<List<OrderDTO>> GetRestaurantOrders(int restaurantId, int currentUserId); // updated
        Task<OrderDTO> GetOrderDetails(int orderId);
        Task<bool> DeleteOrder(int orderId, int userId);
    }
}

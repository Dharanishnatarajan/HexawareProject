using HotPot.DTOs;

namespace HotPot.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCart(int userId);
        Task AddToCart(int userId, AddToCartDTO dto);
        Task RemoveFromCart(int userId, int menuItemId);

        Task UpdateCartItem(int userId, int menuItemId, int quantity);

        Task ClearCart(int userId);
    }
}

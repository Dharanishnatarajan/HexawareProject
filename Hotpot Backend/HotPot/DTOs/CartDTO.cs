namespace HotPot.DTOs
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    public class CartItemDTO
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateCartItemDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }  
    }


    public class AddToCartDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}

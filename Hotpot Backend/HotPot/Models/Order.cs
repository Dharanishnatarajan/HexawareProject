using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Range(0, 100000)]
        public decimal TotalAmount { get; set; }

        [Required, StringLength(255)]
        public string Address { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> OrderItems { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required, Phone]
        public string ContactNumber { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

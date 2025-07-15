using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public decimal? DiscountPrice { get; set; }

        [StringLength(100)]
        public string DietaryInfo { get; set; }

        [StringLength(100)]
        public string TasteInfo { get; set; }

        [StringLength(200)]
        public string NutritionalInfo { get; set; }

        [StringLength(100)]
        public string AvailabilityTime { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public int MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

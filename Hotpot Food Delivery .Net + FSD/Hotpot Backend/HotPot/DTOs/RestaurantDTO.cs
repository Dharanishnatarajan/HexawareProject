using System.ComponentModel.DataAnnotations;

namespace HotPot.DTOs
{
    public class RestaurantDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }
        public string ContactNumber { get; set; }
        public int UserId { get; set; }
    }

    public class CreateRestaurantDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ContactNumber { get; set; }

    }

    public class UpdateRestaurantDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Phone]
        public string ContactNumber { get; set; }
    }



}
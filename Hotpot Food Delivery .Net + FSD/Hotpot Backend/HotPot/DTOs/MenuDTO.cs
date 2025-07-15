namespace HotPot.DTOs
{
    public class MenuItemDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string DietaryInfo { get; set; }
        public string TasteInfo { get; set; }
        public string NutritionalInfo { get; set; }
        public string AvailabilityTime { get; set; }
        public string ImageUrl { get; set; }
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public MenuCategoryDTO MenuCategory { get; set; }
        public RestaurantDTO Restaurant;
    }

    public class CreateMenuItemDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string DietaryInfo { get; set; }
        public string TasteInfo { get; set; }
        public string NutritionalInfo { get; set; }
        public string AvailabilityTime { get; set; }
        public string ImageUrl { get; set; }

        public int MenuCategoryId { get; set; }
        public int RestaurantId { get; set; }
    }

    
        public class CreateMenuCategoryDTO
        {
            public string Name { get; set; }
        }
    


    public class MenuCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
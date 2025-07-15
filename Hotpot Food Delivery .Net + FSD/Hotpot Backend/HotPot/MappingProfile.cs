using AutoMapper;
using HotPot.DTOs;
using HotPot.Models;

namespace HotPot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<RegisterDTO, User>();

            // Restaurant
            CreateMap<CreateRestaurantDTO, Restaurant>();
            CreateMap<Restaurant, RestaurantDTO>();
            CreateMap<UpdateRestaurantDTO, Restaurant>();
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
            CreateMap<Restaurant, RestaurantSummaryDTO>();

            // Menu Category
            CreateMap<MenuCategory, MenuCategoryDTO>();
            CreateMap<CreateMenuCategoryDTO, MenuCategory>();

            // Menu Item
            CreateMap<MenuItem, MenuItemDTO>()
                .ForMember(dest => dest.MenuCategoryName, opt => opt.MapFrom(src => src.MenuCategory.Name))
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant.Name));

            CreateMap<CreateMenuItemDTO, MenuItem>();

            CreateMap<MenuItem, MenuItemSummaryDTO>(); // simplified MenuItem for order

            // Cart
            CreateMap<Cart, CartDTO>();
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.MenuItem.Price));
            CreateMap<AddToCartDTO, CartItem>();

            // Orders
            CreateMap<Order, OrderDTO>();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.MenuItem, opt => opt.MapFrom(src => src.MenuItem)); // simplified MenuItem

            CreateMap<OrderItemDTO, OrderItem>();
        }
    }
}

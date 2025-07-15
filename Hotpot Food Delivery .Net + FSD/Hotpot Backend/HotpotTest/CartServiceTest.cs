using HotPot.Data;
using HotPot.DTOs;
using HotPot.Models;
using HotPot.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HotPot.Tests.Services
{
    public class CartServiceTests
    {
        private HotPotDbContext _context;
        private CartService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotPotDbContext>()
                .UseInMemoryDatabase("CartTestDb")
                .Options;

            _context = new HotPotDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _service = new CartService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddToCart_Should_Add_New_Item()
        {
            var menuCategory = new MenuCategory { Id = 1, Name = "Main Course" };
            var restaurant = new Restaurant
            {
                Id = 1,
                Name = "Test Restaurant",
                ContactNumber = "1234567890",
                Location = "Test Location",
                Description = "Tasty Food",
                UserId = 1
            };

            _context.MenuCategories.Add(menuCategory);
            _context.Restaurants.Add(restaurant);

            var menuItem = new MenuItem
            {
                Id = 1,
                Name = "Pizza",
                Description = "Delicious Pizza",
                Price = 100,
                DiscountPrice = 90,
                DietaryInfo = "Veg",
                TasteInfo = "Spicy",
                NutritionalInfo = "250 kcal",
                AvailabilityTime = "10:00 AM - 10:00 PM",
                ImageUrl = "https://example.com/pizza.jpg",
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                MenuCategoryId = menuCategory.Id,
                MenuCategory = menuCategory
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            var dto = new AddToCartDTO { MenuItemId = 1, Quantity = 2 };
            await _service.AddToCart(10, dto);

            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == 10);

            Assert.NotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual(2, cart.CartItems.First().Quantity);
        }


        [Test]
        public async Task UpdateCartItem_Should_Update_Quantity()
        {
            var userId = 5;
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new System.Collections.Generic.List<CartItem> {
                new CartItem { MenuItemId = 99, Quantity = 1 }
            }
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            await _service.UpdateCartItem(userId, 99, 4);
            var updatedCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            Assert.AreEqual(4, updatedCart.CartItems.First().Quantity);
        }
    }
}

using AutoMapper;
using HotPot.Data;
using HotPot.DTOs;
using HotPot.Models;
using HotPot.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPot.Tests.Services
{
    public class OrderServiceTests
    {
        private HotPotDbContext _context;
        private IMapper _mapper;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotPotDbContext>()
                .UseInMemoryDatabase(databaseName: "HotPotOrderTestDb")
                .Options;

            _context = new HotPotDbContext(options);
            _context.Database.EnsureDeleted(); // Ensure fresh database for each test run
            _context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); // Ensure this is your correct AutoMapper profile
            });

            _mapper = config.CreateMapper();
            _orderService = new OrderService(_context, _mapper);

            // ✅ Seed required data
            _context.Users.Add(new User
            {
                Id = 1,
                Name = "Test User", // Required field
                Role = "Customer"
            });

            _context.Restaurants.Add(new Restaurant
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "Great food place",
                Location = "City Center",
                ContactNumber = "1234567890",
                UserId = 1
            });

            _context.MenuCategories.Add(new MenuCategory
            {
                Id = 1,
                Name = "Main Course"
            });

            _context.MenuItems.Add(new MenuItem
            {
                Id = 1,
                Name = "Pizza",
                Price = 200,
                Description = "Delicious cheese pizza",
                RestaurantId = 1,
                MenuCategoryId = 1,
                AvailabilityTime = "10:00 AM - 10:00 PM",
                ImageUrl = "https://example.com/pizza.png",
                TasteInfo = "Savory",
                DietaryInfo = "Vegetarian",
                NutritionalInfo = "500 kcal"
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task PlaceOrder_Should_Create_Order_And_Clear_Cart()
        {
            // Arrange
            _context.Carts.Add(new Cart
            {
                UserId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        MenuItemId = 1,
                        Quantity = 2,
                        MenuItem = await _context.MenuItems.FindAsync(1)
                    }
                }
            });
            await _context.SaveChangesAsync();

            var createOrder = new CreateOrderDTO
            {
                Address = "123 Test Street",
                PhoneNumber = "9876543210"
            };

            // Act
            var result = await _orderService.PlaceOrder(1, createOrder);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.OrderItems.Count);
            Assert.AreEqual("123 Test Street", result.Address);

            var cartAfter = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == 1);

            Assert.IsEmpty(cartAfter.CartItems);
        }


        [Test]
        public async Task GetUserOrders_Should_Return_UserOrders()
        {
            // Arrange
            _context.Orders.Add(new Order
            {
                Id = 2,
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                Status = "Delivered",
                Address = "Test Addr",
                PhoneNumber = "8888888888",
                TotalAmount = 150,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        MenuItemId = 1,
                        Quantity = 1,
                        Price = 150
                    }
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderService.GetUserOrders(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(1, result.Count);
        }
    }
}

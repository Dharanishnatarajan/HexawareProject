using AutoMapper;
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
    public class MenuServiceTests
    {
        private HotPotDbContext _context;
        private IMapper _mapper;
        private MenuService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotPotDbContext>()
                .UseInMemoryDatabase(databaseName: "HotPotTestDb")
                .Options;

            _context = new HotPotDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            _service = new MenuService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddMenuItem_Should_Add_MenuItem_For_Authorized_User()
        {
            var userId = 1;
            var restaurant = new Restaurant
            {
                Id = 1,
                UserId = userId,
                Name = "TestRestaurant",
                Location = "Test Location",
                ContactNumber = "1234567890",
                Description = "Nice food"
            };
            var category = new MenuCategory { Id = 1, Name = "Starters" };

            await _context.Restaurants.AddAsync(restaurant);
            await _context.MenuCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            var dto = new CreateMenuItemDTO
            {
                Name = "Test Item",
                Description = "Yummy",
                Price = 100,
                DiscountPrice = 90,
                MenuCategoryId = 1,
                RestaurantId = 1,
                DietaryInfo = "Veg",
                TasteInfo = "Spicy",
                NutritionalInfo = "100kcal",
                AvailabilityTime = "10:00 AM - 10:00 PM",
                ImageUrl = "http://test.com/image.png"
            };

            var result = await _service.AddMenuItem(dto, userId);

            Assert.NotNull(result);
            Assert.AreEqual("Test Item", result.Name);
            Assert.AreEqual(1, result.RestaurantId);
        }

        [Test]
        public async Task GetMenuItemsByRestaurant_Should_Return_Items()
        {
            var restaurant = new Restaurant
            {
                Id = 2,
                UserId = 99,
                Name = "Resto 2",
                Location = "Somewhere",
                ContactNumber = "9999999999",
                Description = "Great meals"
            };
            var category = new MenuCategory { Id = 2, Name = "Main Course" };

            await _context.Restaurants.AddAsync(restaurant);
            await _context.MenuCategories.AddAsync(category);

            await _context.MenuItems.AddAsync(new MenuItem
            {
                Name = "Pasta",
                MenuCategoryId = 2,
                RestaurantId = 2,
                Price = 120,
                DiscountPrice = 100,
                Description = "Creamy pasta",
                AvailabilityTime = "10-10",
                DietaryInfo = "",
                TasteInfo = "",
                NutritionalInfo = "",
                ImageUrl = ""
            });

            await _context.SaveChangesAsync();

            var result = await _service.GetMenuItemsByRestaurant(2);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Pasta"));
        }

        [Test]
        public async Task DeleteMenuItem_Should_SoftDelete_If_Authorized()
        {
            var restaurant = new Restaurant
            {
                Id = 3,
                UserId = 777,
                Name = "R3",
                Location = "Loc",
                ContactNumber = "0000000000",
                Description = "Delicious"
            };
            var item = new MenuItem
            {
                Id = 10,
                Name = "Delete Me",
                Restaurant = restaurant,
                RestaurantId = 3,
                MenuCategoryId = 1,
                Price = 200,
                DiscountPrice = 180,
                Description = "Del item",
                AvailabilityTime = "10-10",
                DietaryInfo = "",
                TasteInfo = "",
                NutritionalInfo = "",
                ImageUrl = ""
            };

            await _context.Restaurants.AddAsync(restaurant);
            await _context.MenuItems.AddAsync(item);
            await _context.SaveChangesAsync();

            var deleted = await _service.DeleteMenuItem(10, 777);

            Assert.IsTrue(deleted);
            var deletedItem = await _context.MenuItems.IgnoreQueryFilters().FirstOrDefaultAsync(i => i.Id == 10);
            Assert.IsTrue(deletedItem.IsDeleted);
        }
    }
}

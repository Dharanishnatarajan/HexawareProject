using HotPot.Data;
using HotPot.DTOs;
using HotPot.Models;
using HotPot.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace HotPot.Tests.Services
{
    public class RestaurantServiceTests
    {
        private HotPotDbContext _context;
        private IMapper _mapper;
        private RestaurantService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotPotDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HotPotDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _service = new RestaurantService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddRestaurant_Should_Add_Successfully()
        {
            // Arrange
            var userId = 1;
            var restaurantDto = new CreateRestaurantDTO
            {
                Name = "Test Restaurant",
                Description = "Tasty food",
                Location = "City Center",
                ContactNumber = "9999999999"
            };

            // Act
            var result = await _service.AddRestaurant(restaurantDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Test Restaurant", result.Name);
            Assert.AreEqual(userId, result.UserId);
        }

        [Test]
        public async Task GetAllRestaurants_Should_Return_Added_Restaurants()
        {
            // Arrange
            await _context.Restaurants.AddAsync(new Restaurant
            {
                Name = "A1",
                Description = "D1",
                Location = "L1",
                ContactNumber = "123",
                UserId = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllRestaurants();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.AreEqual("A1", result[0].Name);
        }

        [Test]
        public void AddRestaurant_When_UserAlreadyHasRestaurant_Should_Throw()
        {
            // Arrange
            var userId = 1;
            _context.Restaurants.Add(new Restaurant
            {
                Name = "Existing",
                UserId = userId,
                Description = "Old",
                ContactNumber = "000",
                Location = "Loc"
            });
            _context.SaveChanges();

            var newRestaurantDto = new CreateRestaurantDTO
            {
                Name = "New",
                Description = "Desc",
                Location = "Loc",
                ContactNumber = "0000000000"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddRestaurant(newRestaurantDto, userId));
            Assert.That(ex.Message, Is.EqualTo("User already has a restaurant."));
        }
    }
}

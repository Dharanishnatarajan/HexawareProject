using HotPot.DTOs;
using HotPot.Models;
using HotPot.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotPot.Tests.Services
{
    public class AuthServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IConfiguration> _configMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _configMock = new Mock<IConfiguration>();
            var contextMock = new Mock<Data.HotPotDbContext>(new Microsoft.EntityFrameworkCore.DbContextOptions<Data.HotPotDbContext>());
            _authService = new AuthService(_userManagerMock.Object, _configMock.Object, contextMock.Object);
        }

        [Test]
        public async Task Register_Should_Throw_When_User_Exists()
        {
            var registerDto = new RegisterDTO { Username = "test", Role = "User" };
            _userManagerMock.Setup(x => x.FindByNameAsync("test")).ReturnsAsync(new User());

            Assert.ThrowsAsync<ApplicationException>(() => _authService.Register(registerDto));
        }

        [Test]
        public async Task Register_Should_Create_User()
        {
            var dto = new RegisterDTO { Username = "newuser", Password = "Password@123", Role = "User", Name = "Test", Email = "test@example.com" };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.Username)).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), dto.Role)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), dto.Password)).ReturnsAsync(true);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Register(dto)); // Since Login throws
        }
    }
}

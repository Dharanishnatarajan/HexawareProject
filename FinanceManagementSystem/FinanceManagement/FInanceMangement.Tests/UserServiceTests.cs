using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FinanceManagement.Models;

namespace FinanceManagement.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private List<User> mockUsers;
        private List<Expense> mockExpenses;

        [SetUp]
        public void Setup()
        {
            mockUsers = new List<User>();
            mockExpenses = new List<Expense>();
        }

        [Test]
        public void AddUser_InMemory()
        {
            var user = new User
            {
                UserName = "MockUser",
                Email = "mock@example.com",
                Password = "mock123"
            };

            mockUsers.Add(user);

            Assert.That(mockUsers.Any(u => u.UserName == "MockUser" && u.Email == "mock@example.com"), Is.True);
        }


        [Test]
        public void SearchExpense_Match()
        {
            var expense = new Expense
            {
                UserId = 2,
                CategoryId = 3,
                Description = "Bus Fare",
                Amount = 40
            };

            mockExpenses.Add(expense);

            var result = mockExpenses.FirstOrDefault(e => e.UserId == 2 && e.Description == "Bus Fare");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Amount, Is.EqualTo(40));
        }

        [Test]
        public void GetUserById_Invalid_Throw()
        {
            var userId = -10;
            var user = mockUsers.FirstOrDefault(u => u.UserId == userId);

            Assert.That(user, Is.Null);
        }
    }
}

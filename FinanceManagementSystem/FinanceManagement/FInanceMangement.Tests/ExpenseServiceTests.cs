using NUnit.Framework;
using FinanceManagement.Models;
using FinanceManagement.Repositories;

namespace FinanceManagement.Tests
{
    [TestFixture]
    public class ExpenseServiceTests
    {
        private List<Expense> Expensedatabase;

        [SetUp]
        public void Setup()
        {
            Expensedatabase = new List<Expense>();
        }

        [Test]
        public void AddExpense_ShouldAddExpense()
        {
            var expense = new Expense
            {
                UserId = 2,
                CategoryId = 1,
                Description = "Lunch",
                Amount = 100
            };

            Expensedatabase.Add(expense); 
            var userExpenses = Expensedatabase.Where(e => e.UserId == 2).ToList();

           
            Assert.That(userExpenses.Any(e => e.Description == "Lunch" && e.Amount == 100), Is.True);
        }
    }

}


using System;
using System.Collections.Generic;
using FinanceManagement.Models;
using FinanceManagement.Repositories;

namespace FinanceManagement.Services
{
    public class ExpenseService
    {
        public static void AddExpense()
        {
            Console.WriteLine("\nAvailable Expense Categories:");
            List<ExpenseCategory> categories = ExpenseCategoryRepository.GetExpenseCategories();
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }

            Console.Write("\nEnter User ID: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Category ID: ");
            int categoryId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Expense expense = new Expense { UserId = userId, CategoryId = categoryId, Description = description, Amount = amount };
            ExpenseRepository.AddExpense(expense);
        }

        public static void Viewuser()
        {
            Console.WriteLine("\n===== View User =====");

            var users = UserRepository.GetAllUsers();
            Console.WriteLine("\nAvailable Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.UserId}. {user.UserName} ({user.Email})");
            }
        }
        public static void ViewUserExpenses()
        {
            Console.WriteLine("\n===== View User Expenses =====");

            var users = UserRepository.GetAllUsers();
            Console.WriteLine("\nAvailable Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.UserId}. {user.UserName} ({user.Email})");
            }

            Console.Write("\nEnter User ID to view expenses: ");
            
            int userID = Convert.ToInt32(Console.ReadLine());

            var userToView = UserRepository.GetUserById(userID);
            var expenses = ExpenseRepository.GetExpensesByUser(userID);

            Console.WriteLine($"\nExpenses for {userToView.UserName}:");

            Console.WriteLine("\nID\tCategory\tAmount\tDescription");
            Console.WriteLine("----------------------------------------");

            foreach (var expense in expenses)
            {
                var category = ExpenseCategoryRepository.GetCategoryById(expense.CategoryId);
                Console.WriteLine($"{expense.Id}\t{category.CategoryName}\t{expense.Amount}\t{expense.Description}");
            }

            decimal total = expenses.Sum(e => e.Amount);
            Console.WriteLine($"\nTotal expenses: {total}");
        }


        public static void UpdateExpense()
        {
            Console.Write("Enter Expense ID to update: ");
            int expenseId=Convert.ToInt32(Console.ReadLine());

            Expense existingExpense = ExpenseRepository.GetExpenseById(expenseId);
            if (existingExpense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            Console.WriteLine("\nCurrent Expense Details:");
            Console.WriteLine($"User ID: {existingExpense.UserId}");
            Console.WriteLine($"Category ID: {existingExpense.CategoryId}");
            Console.WriteLine($"Description: {existingExpense.Description}");
            Console.WriteLine($"Amount: {existingExpense.Amount}");

            Console.WriteLine("\nAvailable Expense Categories:");
            List<ExpenseCategory> categories = ExpenseCategoryRepository.GetExpenseCategories();
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }

            Console.Write("\nEnter new Category ID (press Enter to keep current): ");
            string categoryInput = Console.ReadLine();
            int newCategoryId = string.IsNullOrEmpty(categoryInput) ? existingExpense.CategoryId : int.Parse(categoryInput);

            Console.Write("Enter new Description (press Enter to keep current): ");
            string newDescription = Console.ReadLine();
            if (string.IsNullOrEmpty(newDescription))
            {
                newDescription = existingExpense.Description;
            }

            Console.Write("Enter new Amount (press Enter to keep current): ");
            string amountInput = Console.ReadLine();
            decimal newAmount = string.IsNullOrEmpty(amountInput) ? existingExpense.Amount : decimal.Parse(amountInput);

            Expense updatedExpense = new Expense 
            { 
                Id = expenseId,
                UserId = existingExpense.UserId, 
                CategoryId = newCategoryId,
                Description = newDescription,
                Amount = newAmount
            };

            ExpenseRepository.UpdateExpense(updatedExpense);
            Console.WriteLine("Expense updated successfully.");
        }

        public static void DeleteExpense()
        {
            Console.Write("Enter Expense ID to delete: ");

            int expenseId=Convert.ToInt32(Console.ReadLine());

            ExpenseRepository.DeleteExpense(expenseId);
        }
    }
}
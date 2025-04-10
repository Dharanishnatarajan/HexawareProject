using System;
using FinanceManagement.Services;

namespace FinanceManagement
{
    class FinanceApp
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n===== Finance Management System =====");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. Update Expense");
                Console.WriteLine("4. View User");
                Console.WriteLine("5. View Expense");
                Console.WriteLine("6. Delete User");
                Console.WriteLine("7. Delete Expense");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UserService.AddUser();
                        break;
                    case "2":
                        ExpenseService.AddExpense();
                        break;
                    case "3":
                        ExpenseService.UpdateExpense();
                        break;
                    case "4":
                        ExpenseService.Viewuser();
                        break;
                    case "5":
                        ExpenseService.ViewUserExpenses();
                        break;
                    case "6":
                        UserService.DeleteUser();
                        break;
                    case "7":
                        ExpenseService.DeleteExpense();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }
    }
}
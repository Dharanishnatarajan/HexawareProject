using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FinanceManagement.Database;
using FinanceManagement.Models;

namespace FinanceManagement.Repositories
{
    public class ExpenseRepository
    {
        public static void AddExpense(Expense expense)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "INSERT INTO Expenses (UserId, CategoryId, Amount, Description) " +
                               "VALUES (@UserId, @CategoryId, @Amount, @Description)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", expense.UserId);
                    cmd.Parameters.AddWithValue("@CategoryId", expense.CategoryId);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Expense added successfully.");
                }
            }
        }

        public static Expense GetExpenseById(int expenseId)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT ExpenseId, UserId, CategoryId, Amount, Description " +
                              "FROM Expenses WHERE ExpenseId = @ExpenseId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Expense
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                CategoryId = reader.GetInt32(2),
                                Amount = reader.GetDecimal(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static List<Expense> GetExpensesByUser(int userId)
        {
            var expenses = new List<Expense>();

            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT ExpenseId, UserId, CategoryId, Amount, Description " +
                              "FROM Expenses WHERE UserId = @UserId " +
                              "ORDER BY ExpenseId DESC"; 

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            expenses.Add(new Expense
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                CategoryId = reader.GetInt32(2),
                                Amount = reader.GetDecimal(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return expenses;
        }

        // Method to update an expense
        public static void UpdateExpense(Expense expense)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "UPDATE Expenses SET " +
                              "CategoryId = @CategoryId, " +
                              "Amount = @Amount, " +
                              "Description = @Description " +
                              "WHERE ExpenseId = @ExpenseId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expense.Id);
                    cmd.Parameters.AddWithValue("@CategoryId", expense.CategoryId);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine( "Expense updated successfully.");
                }
            }
        }

        // Method to delete an expense by ID
        public static void DeleteExpense(int expenseId)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "DELETE FROM Expenses WHERE ExpenseId = @ExpenseId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Expense deleted successfully.");
                }
            }
        }
    }
}
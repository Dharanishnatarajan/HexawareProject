using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FinanceManagement.Database;
using FinanceManagement.Models;

namespace FinanceManagement.Repositories
{
    public class ExpenseCategoryRepository
    {
        public static List<ExpenseCategory> GetExpenseCategories()
        {
            var categories = new List<ExpenseCategory>();

            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT CategoryId, CategoryName FROM ExpenseCategories";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new ExpenseCategory
                        {
                            CategoryId = reader.GetInt32(0),
                            CategoryName = reader.GetString(1)
                        });
                    }
                }
            }

            return categories;
        }

        public static ExpenseCategory GetCategoryById(int categoryId)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT CategoryId, CategoryName FROM ExpenseCategories WHERE CategoryId = @CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ExpenseCategory
                            {
                                CategoryId = reader.GetInt32(0),
                                CategoryName = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}

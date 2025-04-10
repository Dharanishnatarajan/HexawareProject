using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FinanceManagement.Database;
using FinanceManagement.Models;

namespace FinanceManagement.Repositories
{
    public class UserRepository
    {
        
        public static void AddUser(User user)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "INSERT INTO Users (UserName, Email, Password) VALUES (@UserName, @Email, @Password)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("User added successfully.");
                }
            }
        }


        
        public static void DeleteUser(int userId)
        {
            using (var conn = DbConnectionHelper.GetConnection())  
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("User deleted successfully.");
                }
            }
        }

        public static List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT UserId, UserName, Email FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            Email = reader.GetString(2)
                        });
                    }
                }
            }

            return users;
        }

        public static User GetUserById(int userId)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                string query = "SELECT UserId, UserName, Email FROM Users WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Email = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}

using System;
using FinanceManagement.Models;
using FinanceManagement.Repositories;

namespace FinanceManagement.Services
{
    public class UserService
    {
        public static void AddUser()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine(); 

            User newUser = new User
            {
                UserName = username,
                Email = email,
                Password = password  
            };

            UserRepository.AddUser(newUser); 
   
        }

        public static void DeleteUser()
        {
            Console.Write("Enter User ID to delete: ");

            int userId=Convert.ToInt32(Console.ReadLine());

            UserRepository.DeleteUser(userId);
        }
    }
}

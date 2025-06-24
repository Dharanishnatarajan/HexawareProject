using HotPot.Models;
using Microsoft.AspNetCore.Identity;

namespace HotPot.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAndRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Define all roles to seed
            string[] roles = { "Admin", "User", "Restaurant" };

            // Seed each role if not exists
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }

            // Seed Admin user if not exists
            if (await userManager.FindByNameAsync("admin") is null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@hotpot.com",
                    Name = "Super Admin",
                    Role = "Admin",
                    Gender = "Male",
                    Address = "System"
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}

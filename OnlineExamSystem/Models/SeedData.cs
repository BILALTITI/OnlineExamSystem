using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles if they don't exist
            string[] roles = { "Admin", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create Admin User
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // (Optional) Create Student User
            var studentUser = await userManager.FindByNameAsync("student");
            if (studentUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "student",
                    Email = "student@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Student@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Student");
                }
            }
        }
    }
}

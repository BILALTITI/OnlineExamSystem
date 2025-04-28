//// Data/SeedData.cs
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
// using System;
//using System.Threading.Tasks;

//public static class SeedData
//{
//    public static async Task Initialize(IServiceProvider serviceProvider)
//    {
//        // Get required services
//        var context = serviceProvider.GetRequiredService<ApplicationUser>();
//        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
//        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//        // Ensure database is created/migrated
//        await context.Database.MigrateAsync();

//        // Seed Roles
//        await SeedRoles(roleManager);

//        // Seed Admin User
//        await SeedAdminUser(userManager);
//    }

//    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
//    {
//        // Create Admin Role if not exists
//        if (!await roleManager.RoleExistsAsync("Admin"))
//        {
//            await roleManager.CreateAsync(new IdentityRole("Admin"));
//            await roleManager.CreateAsync(new IdentityRole("User"));
//        }
//    }

//    private static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
//    {
//        // Check if admin user exists
//        const string adminEmail = "admin@example.com";
//        var adminUser = await userManager.FindByEmailAsync(adminEmail);

//        if (adminUser == null)
//        {
//            // Create new admin user
//            adminUser = new IdentityUser
//            {
//                UserName = adminEmail,
//                Email = adminEmail,
//                EmailConfirmed = true,
//                PhoneNumberConfirmed = true
//            };

//            // Create user with password
//            var createResult = await userManager.CreateAsync(adminUser, "Admin@1234!");

//            if (!createResult.Succeeded)
//            {
//                throw new Exception("Admin user creation failed: " +
//                    string.Join(", ", createResult.Errors));
//            }

//            // Assign admin role
//            var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

//            if (!roleResult.Succeeded)
//            {
//                throw new Exception("Admin role assignment failed: " +
//                    string.Join(", ", roleResult.Errors));
//            }
//        }
//    }
//}
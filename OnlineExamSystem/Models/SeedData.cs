using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created and migrated
        await context.Database.MigrateAsync();

        // Seed Roles
        await SeedRoles(roleManager);

        // Seed Admin User
        await SeedAdminUser(userManager);

        // Seed Sample Exams
        await SeedSampleExams(context);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roleNames = new[] { "Admin", "Student" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
    {
        const string adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    private static async Task SeedSampleExams(ApplicationDbContext context)
    {
        if (!context.Exams.Any())
        {
            var exams = new List<Exam>
            {
                new Exam
                {
                    Title = "C# Fundamentals",
                    Description = "Basic C# Programming Exam",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Which keyword is used to define a class in C#?",
                            Option1 = "class",
                            Option2 = "struct",
                            Option3 = "interface",
                            Option4 = "namespace",
                            CorrectAnswer = 1
                        },
                        new Question
                        {
                            Text = "What is the base class for all objects in C#?",
                            Option1 = "System.Base",
                            Option2 = "System.Object",
                            Option3 = "System.Root",
                            Option4 = "System.Parent",
                            CorrectAnswer = 2
                        }
                    }
                },
                new Exam
                {
                    Title = "ASP.NET Core Basics",
                    Description = "Web Development Fundamentals",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Which method configures services in Startup.cs?",
                            Option1 = "ConfigureServices",
                            Option2 = "Configure",
                            Option3 = "InitializeServices",
                            Option4 = "SetupServices",
                            CorrectAnswer = 1
                        }
                    }
                }
            };

            await context.Exams.AddRangeAsync(exams);
            await context.SaveChangesAsync();
        }
    }
}
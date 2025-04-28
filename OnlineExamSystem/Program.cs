using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Areas;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;
using OnlineExamSystem.Models.Repositroy;

namespace OnlineExamSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder. Services.AddControllersWithViews()
      .AddMvcOptions(options =>
          options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure cookie settings
            builder.Services.ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Admins/Account/Login"; // Login path for unauthenticated users
                options.AccessDeniedPath = "/Admins/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Register repositories
            builder.Services.AddScoped<IExam, ExamRepositroy>();
            builder.Services.AddScoped<IQuestion, QuestionRepositroy>();
            builder.Services.AddScoped<IUserExamResult, StudentExamRepository>();

            // CORS Policy (if needed)
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Area Route (for Admins)
            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Default Route (forces login page)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}",
                defaults: new { area = "Admins" });

            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                await SeedData.Initialize(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

             builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

             builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

             builder.Services.ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Admins/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Register repositories
            builder.Services.AddScoped<IExam, ExamRepositroy>();
            builder.Services.AddScoped<IQuestion, QuestionRepositroy>();
            builder.Services.AddScoped<IUserExamResult, StudentExamRepository>();

          

            var app = builder.Build();

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

            app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

 
            app.MapControllers(); 
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
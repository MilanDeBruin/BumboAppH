using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.App.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<BumboDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Bumbo")));
            
            builder.Services.AddTransient<Bumbo.Data.Interfaces.INormRepository, Bumbo.Data.SqlRepository.NormRepository>();
            builder.Services.AddTransient<Bumbo.Data.Interfaces.IForecastRepository, Bumbo.Data.SqlRepository.ForecastRepository>();
            builder.Services.AddTransient<Bumbo.Domain.Services.Forecast.IGenerateForecastService, Bumbo.Domain.Services.Forecast.GenerateForecastService>();

            builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
                
                options.User.RequireUniqueEmail = true;
            });
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "login",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}

using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            builder.Services.AddTransient<Bumbo.Data.Interfaces.ILeaveRepository, Bumbo.Data.SqlRepository.LeaveRepository>();
            builder.Services.AddTransient<Bumbo.Data.Interfaces.IEmployeeRepository, Bumbo.Data.SqlRepository.EmployeeRepository>();
            builder.Services.AddTransient<Bumbo.Data.Interfaces.INormRepository, Bumbo.Data.SqlRepository.NormRepository>();
            builder.Services.AddTransient<Bumbo.Data.Interfaces.IForecastRepository, Bumbo.Data.SqlRepository.ForecastRepository>();
            builder.Services.AddTransient<Bumbo.Domain.Services.Forecast.IGenerateForecastService, Bumbo.Domain.Services.Forecast.GenerateForecastService>();
            builder.Services.AddTransient<Bumbo.Domain.Services.Leaves.ILeaveChecker, Bumbo.Domain.Services.Leaves.LeaveChecker>();




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

            app.UseStatusCodePagesWithReExecute("/Home/Error404");

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

namespace Bumbo.App.Web.Controllers;

using System.Security.Claims;
using Models.ViewModels.Forecast;
using Bumbo.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly BumboDbContext _context;

    public AccountController(BumboDbContext bumboDbContext)
    {
        this._context = bumboDbContext;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if(User.Identity.IsAuthenticated)
        {
            return this.RedirectToAction("Index", "Home");
        }
        var model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        // Check if the model state is valid
        if (!this.ModelState.IsValid)
        {
            return this.View(viewModel);
        }

        // Attempt to find the employee with the provided email and password
        var employee = await this._context.Employees
            .FirstOrDefaultAsync(e => e.EmailAdres == viewModel.Email && e.Password == viewModel.Password);

        if (employee != null)
        {
            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, viewModel.Email),
                new Claim("position", employee.Position),
                new Claim("branch_id", employee.BranchId.ToString()),
                new Claim("employee_id", employee.EmployeeId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user with the created claims
            await this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redirect to the Home page upon successful login
            return this.RedirectToAction("Index", "Home");
        }

        // Add an error message to the model state if authentication fails
        this.ModelState.AddModelError(string.Empty, "Invalid email or password");
        return this.View(viewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return this.RedirectToAction("Login", "Account");
    }
}
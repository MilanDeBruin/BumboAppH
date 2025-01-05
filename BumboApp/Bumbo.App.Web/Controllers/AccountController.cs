using Bumbo.App.Web.Models.ViewModels;
using Bumbo.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.App.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class AccountController : Controller
{
    private readonly BumboDbContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(BumboDbContext dbContext, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _context = dbContext;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {

            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, false);

            if (result.Succeeded)
            {
                Response.Cookies.Append("employee_id", viewModel.Email, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(viewModel);
            }
        }

        return this.View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    // Method for development purposes
    private async Task createUser(string userName, string password)
    {
        var result = await _userManager.CreateAsync(new IdentityUser(userName), password);
        
        if (result.Succeeded)
        {
            Console.WriteLine("User created");
        }
        else
        {
            Console.WriteLine("User not created");
        }
    }
}
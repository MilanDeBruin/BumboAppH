using Bumbo.App.Web.Models.ViewModels.Forecast;
using Bumbo.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers;

[Authorize]
public class AccountController_3 : Controller
{
    private readonly BumboDbContext _context;
    private readonly SignInManager<IdentityUser<int>> _signInManager;

    public AccountController_3(BumboDbContext context, SignInManager<IdentityUser<int>> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
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
}
using System.Security.Claims;
using Bumbo.App.Web.Models.ViewModels;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
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
    private readonly IEmployeeRepository _employeeRepository;

    public AccountController(BumboDbContext dbContext, SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager, IEmployeeRepository employeeRepository)
    {
        _context = dbContext;
        _signInManager = signInManager;
        _userManager = userManager;
        _employeeRepository = employeeRepository;
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
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, false); // TODO: Shouldn't be async

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(viewModel);
                }

                var employee = _employeeRepository.GetEmployee(user.Id);
                if (employee == null)
                {
                    ModelState.AddModelError(string.Empty, "Employee not found.");
                    return View(viewModel);
                }

                var claims = new List<Claim>
                {
                    new Claim("employee_id", employee.EmployeeId.ToString()),
                    new Claim("branch_id", employee.BranchId.ToString()),
                    new Claim("user_id", user.Id),
                    new Claim("position", _employeeRepository.getRoles(user.Id).ToLower())
                };
                
                var claimsResult = await _userManager.AddClaimsAsync(user, claims);
                
                if (!claimsResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add claims.");
                    return View(viewModel);
                }
                
                // Reload the user's claims, so the new claims are available
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(viewModel);
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
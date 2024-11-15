using System.Collections.Immutable;
using BumboApp.Models.Models;
using BumboApplicatie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bumbo.App.Web.Models.Services;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApplicatie.Models;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BumboDbContext _db;

        public HomeController(ILogger<HomeController> logger, BumboDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Prognose()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

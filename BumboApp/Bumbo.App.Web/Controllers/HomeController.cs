using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bumbo.App.Web.Models;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using BumboApplicatie.Models;

namespace Bumbo.App.Web.Controllers
{
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
            var Werktijd = _db.WorkSchedules
                //.Where(Ws => Ws.EmployeeId == 2)
                .Select(Ws => new
                {
                    Ws.EmployeeId,
                    Ws.Date,
                    Ws.StartTime,
                    Ws.EndTime,
                    Ws.BranchId,
                    Ws.Department

                }).ToList();
            Console.WriteLine("test");
            Console.WriteLine(Werktijd[0]);
            return View(Werktijd);
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

        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("404");
        }

    }
}

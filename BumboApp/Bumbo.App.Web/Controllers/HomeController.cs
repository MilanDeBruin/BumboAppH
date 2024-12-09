using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bumbo.Data.Context;
using Bumbo.Domain.Services.CAO;
using BumboApplicatie.Models;
using Microsoft.VisualBasic;
using Bumbo.Domain.Models;
using Bumbo.App.Web.Models.ViewModels.Home;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.App.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repo;
        
        
        public HomeController(IHomeRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            WeekPersonalScheduleViewModel viewModel = new WeekPersonalScheduleViewModel
            {
                FirstDayOfWeek = date
            };
            viewModel.WorkDays = new List<DayPersonalScheduleViewModel>();
            List<WorkSchedule> schedules = _repo.GetScheduleData(1, DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Today)));
            
            foreach (var schedule in schedules)
            {
                DayPersonalScheduleViewModel model = new DayPersonalScheduleViewModel
                {
                    date = schedule.Date,
                    StartTime = schedule.StartTime,
                    endTime = schedule.EndTime,
                    Departement = schedule.Department,
                    Branch_Id = schedule.BranchId,
                    Is_Sick = schedule.IsSick,
                };
                viewModel.WorkDays.Add(model);            

            }
            viewModel.isSick = _repo.GetSick(1); //cookies id nog toevoegen
            return View(viewModel);
           
        }

        [HttpGet]
        public IActionResult Ziekmelden()
        {
            DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            _repo.SetSick(1, date); //toevoegen cookies gezijk
            TempData["SuccessMessage"] = "Je bent ziekgemeld!";
            return RedirectToAction("Index");
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

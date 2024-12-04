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
                    Branch_Id = schedule.BranchId
                };
                viewModel.WorkDays.Add(model);

            }
            return View(viewModel);
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

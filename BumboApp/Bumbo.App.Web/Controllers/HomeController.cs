using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using BumboApplicatie.Models;
using Microsoft.VisualBasic;
using Bumbo.Domain.Models;
using Bumbo.App.Web.Models.ViewModels.Home;
using Bumbo.Data.Interfaces;

namespace Bumbo.App.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repo;
        public int id_employee = 1;
        
        public HomeController(IHomeRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            WeekPersonalScheduleViewModel viewModel = new WeekPersonalScheduleViewModel();
            List<WorkSchedule> schedules = _repo.GetScheduleData(1, DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Today)));

            DayPersonalScheduleViewModel model = new DayPersonalScheduleViewModel();

            model.date = schedules[0].Date; 
            model.endTime = schedules[0].EndTime;
            model.StartTime = schedules[0].StartTime;
            model.Departement = schedules[0].Department;
            model.Branch_Id = schedules[0].BranchId;

            viewModel. = model;


            
            
            
           



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

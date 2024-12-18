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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Bumbo.App.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repo;
        
        public HomeController(IHomeRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index(string? date)
        {
            var v = User.FindFirst("employee_id")?.Value;
            int employeeId = int.Parse(v);

            DateOnly firstDayOfWeek = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));

            if (date != null)
            {
                firstDayOfWeek = DateOnly.Parse(date);
            }

            var viewModel = new WeekPersonalScheduleViewModel
                {
                    FirstDayOfWeek = firstDayOfWeek,
                    WorkDays = new List<DayPersonalScheduleViewModel>()
                };
                List<WorkSchedule> schedules = _repo.GetScheduleData(employeeId, firstDayOfWeek);

                 var groupedSchedules = schedules
                .GroupBy(schedule => schedule.Date)
                .OrderBy(group => group.Key);

                foreach (var group in groupedSchedules)
                {
                    var daySchedule = new DayPersonalScheduleViewModel
                    {
                        Date = group.Key,
                        Shifts = group.Select(schedule => new ShiftsViewModel
                        {
                            Time = $"{schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}",
                            Departement = schedule.Department,
                            Branch_Id = schedule.BranchId
                        }).ToList()
                    };
                    viewModel.WorkDays.Add(daySchedule);

                }
                viewModel.isSick = _repo.GetSick(employeeId); 
                return View(viewModel);
           
        }

        [HttpGet]
        public IActionResult Ziekmelden(int employeeId)
        {

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            _repo.SetSick(employeeId, date);
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

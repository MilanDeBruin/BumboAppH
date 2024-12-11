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
                List<WorkSchedule> schedules = _repo.GetScheduleData(1, firstDayOfWeek); //cookies toevoegen ook

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
                viewModel.isSick = _repo.GetSick(1); //cookies id nog toevoegen
                return View(viewModel);
           
        }

        [HttpGet]
        public IActionResult Ziekmelden()
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            _repo.SetSick(1, date); //toevoegen cookies gezijk
            TempData["SuccessMessage"] = "Je bent ziekgemeld!";
            return RedirectToAction("Index");
        }    

        //public IActionResult WeekForward()
        //{
        //    Console.WriteLine(firstDayOfWeek);
        //    firstDayOfWeek = firstDayOfWeek.AddDays(7);
        //    Console.WriteLine(firstDayOfWeek);

        //    return RedirectToAction("Index");

        //}

        //public IActionResult WeekBackward()
        //{
        //    Console.WriteLine(firstDayOfWeek);
        //    firstDayOfWeek = firstDayOfWeek.AddDays(-7);
        //    Console.WriteLine(firstDayOfWeek);

        //    return RedirectToAction("Index");

        //}

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

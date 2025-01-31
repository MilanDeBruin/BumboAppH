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
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
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
                        Time = $"{schedule.StartTime:HH:mm} - {schedule.EndTime:HH:mm}".ToString(),
                        Departement = schedule.Department,
                        Branch_Id = schedule.BranchId,
                        Is_Sick = schedule.IsSick
                    }).ToList()
                };
                viewModel.WorkDays.Add(daySchedule);

            }
            viewModel.ingeklokt = _repo.GetIngeklokt(employeeId);
            viewModel.isSick = _repo.GetSick(employeeId);
            viewModel.sickListNames = _repo.getSickList();
            return View(viewModel);

        }

        [HttpGet]
        public IActionResult Ziekmelden(int employeeId)
        {
            if (_repo.GetIngeklokt(employeeId) == true)
            {
                TempData["WarningMessage"] = "Je bent ingeklokt en mag niet ziekmelden!";
                return RedirectToAction("Index");
            }
            if (_repo.CheckShift(employeeId) == false)
            {
                TempData["WarningMessage"] = "je hebt geen dienst vandaag.";
                return RedirectToAction("Index");
            }
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            _repo.SetSick(employeeId, date);
            TempData["SuccessMessage"] = "Je bent ziekgemeld!";
            return RedirectToAction("Index");
        }

        public IActionResult Inklokken(int employeeId)
        {
            var currentTime = DateTime.Now;
            int minutes = currentTime.Minute;

            int roundedMinutes = (int)(Math.Round(minutes / 15.0) * 15);
            if (roundedMinutes == 60)
            {
                currentTime = currentTime.AddHours(1);
                roundedMinutes = 0;
            }

            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, roundedMinutes, 0);

            if (_repo.GetSick(employeeId) == true)
            {
                TempData["WarningMessage"] = "Je bent ziek en mag niet inklokken!";
                return RedirectToAction("Index");
            }
            if (_repo.CheckStartTime(employeeId, currentTime))
            {
                TempData["WarningMessage"] = "Je bent al ingeklokt binnen dit kwartier!";
                return RedirectToAction("Index");
            }
            else
            {
                _repo.Inklokken(employeeId, currentTime);
                TempData["SuccessMessage"] = "Je bent ingeklokt!";
                return RedirectToAction("Index");
            }
           
        }

        public IActionResult Uitklokken(int employeeId)
        {
            var currentTime = DateTime.Now;
            int minutes = currentTime.Minute;

            int roundedMinutes = (int)(Math.Round(minutes / 15.0) * 15);
            if (roundedMinutes == 60)
            {
                currentTime = currentTime.AddHours(1);
                roundedMinutes = 0;
            }
            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, roundedMinutes, 0);

            if (_repo.CheckStartTime(employeeId, currentTime))
            {
                TempData["WarningMessage"] = "Je shift was korter dan een kwartier en is niet opgeslagen";
                _repo.DeleteShift(employeeId, currentTime);
                return RedirectToAction("Index");
            }
            _repo.Uitklokken(employeeId, currentTime);
            TempData["SuccessMessage"] = "Je bent uitgeklokt!";
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

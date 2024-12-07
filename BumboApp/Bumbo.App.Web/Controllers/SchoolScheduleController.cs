using Bumbo.App.Web.Models.ViewModels.SchoolSchedule;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class SchoolScheduleController(BumboDbContext context, ISchoolScheduleRepository schoolScheduleRepository) : Controller
    {
        private readonly BumboDbContext _context = context;
        private readonly ISchoolScheduleRepository _schoolScheduleRepository = schoolScheduleRepository;

        public IActionResult Details(int employeeId, string? firstDayOfWeek)
        {
            DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            if (firstDayOfWeek != null) date = DateOnly.Parse(firstDayOfWeek);

            List<SchoolSchedule> weekSchoolSchedule =
                _schoolScheduleRepository.GetWeekSchoolScheduleForEmployee(employeeId, date).OrderBy(f => f.Weekday).ToList();

            SchoolScheduleViewModel viewModel = new()
            {
                EmployeeId = employeeId,
                DailySchoolSchedules = []
            };

            if (weekSchoolSchedule.IsNullOrEmpty()) return View(viewModel);

            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = date.AddDays(i);
                var dailySchoolSchedule = new DailySchoolSchedule
                {
                    Weekday = dayDate,
                    StartTime = null,
                    EndTime = null,
                };

                var employeeSchoolSchedule = weekSchoolSchedule.FirstOrDefault(ss =>
                    ss.Weekday.Equals(dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")), StringComparison.CurrentCultureIgnoreCase));
            
                if (employeeSchoolSchedule != null)
                {
                    dailySchoolSchedule.StartTime = employeeSchoolSchedule.StartTime;
                    dailySchoolSchedule.EndTime = employeeSchoolSchedule.EndTime;
                }

                viewModel.DailySchoolSchedules.Add(dailySchoolSchedule);
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int employeeId)
        {
            DateOnly startDate = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));

            var existingSchoolSchedules = _context.SchoolSchedules
                .Where(ss => ss.EmployeeId == employeeId)
                .ToList();

            var viewModel = new SchoolScheduleViewModel
            {
                EmployeeId = employeeId,
                DailySchoolSchedules = [],
            };

            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = startDate.AddDays(i);

                var dailySchoolSchedule = new DailySchoolSchedule
                {
                    Weekday = dayDate,
                    StartTime = null,
                    EndTime = null,
                };

                var employeeSchoolSchedule = existingSchoolSchedules.FirstOrDefault(ss =>
                    ss.Weekday.Equals(dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")), StringComparison.CurrentCultureIgnoreCase));

                if (employeeSchoolSchedule != null)
                {
                    dailySchoolSchedule.StartTime = employeeSchoolSchedule.StartTime;
                    dailySchoolSchedule.EndTime = employeeSchoolSchedule.EndTime;
                }

                viewModel.DailySchoolSchedules .Add(dailySchoolSchedule);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(SchoolScheduleViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            // Alle schooldagen één voor één opslaan
            foreach (var dailySchoolSchedule in viewModel.DailySchoolSchedules)
            {
                string weekdayName = dailySchoolSchedule.Weekday
                    .ToString("dddd", new System.Globalization.CultureInfo("nl-NL"))
                    .ToLower();

                var schoolSchedule = _context.SchoolSchedules
                    .FirstOrDefault(ss => ss.EmployeeId == viewModel.EmployeeId &&
                        ss.Weekday.ToLower() == weekdayName);

                // Checkt of de schooldag al in de database staat
                if (dailySchoolSchedule.StartTime.HasValue && dailySchoolSchedule.EndTime.HasValue)
                {
                    if (schoolSchedule == null)
                    {
                        _context.SchoolSchedules.Add(new SchoolSchedule
                        {
                            EmployeeId = viewModel.EmployeeId,
                            Weekday = weekdayName,
                            StartTime = dailySchoolSchedule.StartTime,
                            EndTime = dailySchoolSchedule.EndTime,
                        });
                    }
                    else
                    {
                        schoolSchedule.StartTime = dailySchoolSchedule.StartTime.Value;
                        schoolSchedule.EndTime = dailySchoolSchedule.EndTime.Value;
                    }
                }
                // Schooldag heeft geen start- of eindtijd
                else if (schoolSchedule != null) _context.SchoolSchedules.Remove(schoolSchedule);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Schoolrooster is succesvol bijgewerkt!";
            return RedirectToAction("Details", new { employeeId = viewModel.EmployeeId });
        }
    }
}

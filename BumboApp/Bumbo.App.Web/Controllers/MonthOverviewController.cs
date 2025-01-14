using Bumbo.App.Web.Models.ViewModels;
using Bumbo.App.Web.Models.ViewModels.Dayoverview;
using Bumbo.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.App.Web.Controllers
{
    public class MonthOverviewController : Controller
    {
        private readonly BumboDbContext _context;

        public MonthOverviewController(BumboDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string month)
        {
            var selectedMonth = string.IsNullOrEmpty(month)
                ? DateOnly.FromDateTime(DateTime.Today)
                : DateOnly.ParseExact(month, "yyyy-MM");

            var firstDayOfMonth = new DateOnly(selectedMonth.Year, selectedMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var employees = await _context.Employees.ToListAsync();
            var workSchedules = await _context.WorkSchedules
                .Where(ws => ws.Date >= firstDayOfMonth && ws.Date <= lastDayOfMonth)
                .ToListAsync();

            var monthlyOverview = new List<DayOverviewViewModel>();

            foreach (var date in Enumerable.Range(0, (lastDayOfMonth.DayNumber - firstDayOfMonth.DayNumber) + 1)
                                          .Select(offset => firstDayOfMonth.AddDays(offset)))
            {
                foreach (var employee in employees)
                {
                    var workSchedule = workSchedules.FirstOrDefault(ws => ws.EmployeeId == employee.EmployeeId && ws.Date == date);
                    var plannedHours = workSchedule != null ? (decimal)(workSchedule.EndTime - workSchedule.StartTime).TotalHours : 0;
                    var workedHours = 0m;

                    if (plannedHours > 0 || workedHours > 0)
                    {
                        monthlyOverview.Add(new DayOverviewViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            PlannedHours = plannedHours,
                            WorkedHours = workedHours,
                            Date = date,
                        });
                    }
                }
            }

            ViewData["SelectedMonth"] = selectedMonth;
            return View(monthlyOverview);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Dictionary<int, decimal> WorkedHours)
        {
            // Update de gewerkte uren voor elke werknemer
            foreach (var entry in WorkedHours)
            {
                var employeeId = entry.Key;
                var workedHours = entry.Value;

                var workSchedules = await _context.WorkSchedules
                    .Where(ws => ws.EmployeeId == employeeId && ws.Date.Month == DateTime.Today.Month && ws.Date.Year == DateTime.Today.Year)
                    .ToListAsync();

                foreach (var workSchedule in workSchedules)
                {
                    //workSchedule. = workedHours; // Werk de gewerkte uren bij
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "De gewerkte uren zijn succesvol opgeslagen!";
            return RedirectToAction("Index");
        }
    }
}

using Bumbo.App.Web.Models.ViewModels;
using Bumbo.App.Web.Models.ViewModels.Dayoverview;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

            foreach (var employee in employees)
            {
                decimal totalPlannedHours = 0;
                decimal totalWorkedHours = 0;

                foreach (var date in Enumerable.Range(0, (lastDayOfMonth.DayNumber - firstDayOfMonth.DayNumber) + 1)
                                               .Select(offset => firstDayOfMonth.AddDays(offset)))
                {
                    var workSchedule = workSchedules.FirstOrDefault(ws => ws.EmployeeId == employee.EmployeeId && ws.Date == date);
                    var plannedHours = workSchedule != null ? (decimal)(workSchedule.EndTime - workSchedule.StartTime).TotalHours : 0;
                    var workedHours = workSchedules
                                    .Where(ws => ws.EmployeeId == employee.EmployeeId && ws.Date == date)
                                    .Select(ws => ws.EndTime - ws.StartTime)
                                    .FirstOrDefault();

                    totalPlannedHours += plannedHours;
                    totalWorkedHours += (decimal)workedHours.TotalHours;
                }

                if (totalPlannedHours > 0 || totalWorkedHours > 0)
                {
                    monthlyOverview.Add(new DayOverviewViewModel
                    {
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        PlannedHours = totalPlannedHours,
                        WorkedHours = totalWorkedHours,
                    });
                }
            }

            // **Groeperen op voor- en achternaam en geplande uren optellen**
            var groupedOverview = monthlyOverview
                .GroupBy(e => new { e.FirstName, e.LastName })
                .Select(g => new DayOverviewViewModel
                {
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    PlannedHours = g.Sum(e => e.PlannedHours),
                    WorkedHours = g.Sum(e => e.WorkedHours),
                })
                .ToList();

            ViewData["SelectedMonth"] = selectedMonth;
            return View(groupedOverview);
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

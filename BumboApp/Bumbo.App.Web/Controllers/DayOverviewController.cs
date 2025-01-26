using Bumbo.App.Web.Models.ViewModels.Dayoverview;
using Bumbo.App.Web.Models.ViewModels.Home;
using Bumbo.Data.Context;

using Bumbo.Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bumbo.App.Web.Controllers
{
	public class DayOverviewController : Controller
	{
		private readonly BumboDbContext _context;

        private readonly ILogger<DayOverviewController> _logger;

        public DayOverviewController(BumboDbContext context, ILogger<DayOverviewController> logger)
		{
			_context = context;
            _logger = logger;

        }

        public async Task<IActionResult> Index(string? date, string? search)
        {
            _logger.LogInformation("open action triggered...");

            DateTime selectedDate;
            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out selectedDate))
            {
                selectedDate = DateTime.Today;
            }

            var employees = await _context.Employees.ToListAsync();
            var workSchedules = await _context.WorkSchedules
                .Where(ws => ws.Date == DateOnly.FromDateTime(selectedDate))
                .ToListAsync();

            var workShifts = await _context.WorkShifts
                .Where(ws => ws.StartTime.Date == selectedDate)
                .ToListAsync();

            var viewModel = employees
                .Where(e => string.IsNullOrEmpty(search) ||
                            e.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            e.LastName.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Select(e => new DayOverviewViewModel
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    PlannedHours = workSchedules
                        .Where(ws => ws.EmployeeId == e.EmployeeId)
                        .Sum(ws => (decimal)(ws.EndTime - ws.StartTime).TotalHours),
                    WorkedHoursString = TimeSpan.FromMinutes(
                        workShifts
                        .Where(ws => ws.EmployeeId == e.EmployeeId)
                        .Sum(ws => (ws.EndTime - ws.StartTime)?.TotalMinutes ?? 0))
                        .ToString(@"hh\:mm") ?? "00:00",
                    Shifts = workShifts
                        .Where(ws => ws.EmployeeId == e.EmployeeId)
                        .Select(ws => new ShiftViewModel
                        {
                            StartTime = ws.StartTime.ToString("hh:mm"),
                            EndTime = ws.EndTime?.ToString("hh:mm") ?? "N/A"
                        }).ToList()
                })
                .ToList();

            ViewData["Date"] = DateOnly.FromDateTime(selectedDate);
            ViewData["Search"] = search;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(List<DayOverviewViewModel> updatedHours, DateOnly date)
        {
            _logger.LogInformation("Save action triggered... -------------------------------------------------------------------------");

            foreach (var update in updatedHours)
            {
                if (!TimeSpan.TryParse(update.WorkedHoursString, out var workedTimeSpan))
                {
                    _logger.LogWarning($"Invalid time format for EmployeeId {update.EmployeeId}. Skipping...");
                    continue;
                }

                
                var workShifts = await _context.WorkShifts
                    .Where(ws => ws.EmployeeId == update.EmployeeId && ws.StartTime.Date == date.ToDateTime(TimeOnly.MinValue).Date)
                    .ToListAsync();

                
                if (workedTimeSpan == TimeSpan.Zero && workShifts.Any())
                {
                    _logger.LogInformation($"EmployeeId {update.EmployeeId} has no worked hours for the selected date. Removing all shifts...");

                    _context.WorkShifts.RemoveRange(workShifts);  
                    continue;  
                }

                if (workShifts.Any())
                {
                   
                    var totalExistingWorkedTime = workShifts
                        .Skip(1) 
                        .Sum(ws => (ws.EndTime - ws.StartTime)?.TotalMinutes ?? 0);

                    
                    var remainingWorkedMinutes = workedTimeSpan.TotalMinutes - totalExistingWorkedTime;

                    
                    if (remainingWorkedMinutes > 0)
                    {
                        var firstShift = workShifts.First();
                        firstShift.EndTime = firstShift.StartTime.AddMinutes(remainingWorkedMinutes);

                        
                        _context.WorkShifts.Update(firstShift);
                    }
                    else
                    {
                        _logger.LogWarning($"Total worked time is less than the existing shifts' time for EmployeeId {update.EmployeeId}. Skipping update.");
                        continue;
                    }
                }
                else
                {
                    
                    var newWorkShift = new WorkShift
                    {
                        EmployeeId = update.EmployeeId,
                        StartTime = date.ToDateTime(TimeOnly.MinValue),
                        EndTime = date.ToDateTime(TimeOnly.MinValue).Add(workedTimeSpan)
                    };
                    _context.WorkShifts.Add(newWorkShift);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { date });
        }



    }
}

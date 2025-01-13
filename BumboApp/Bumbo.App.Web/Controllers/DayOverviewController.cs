using Bumbo.App.Web.Models.ViewModels.Dayoverview;
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
    .Select(e => new
    {
        EmployeeId = e.EmployeeId,
        FirstName = e.FirstName,
        LastName = e.LastName,
        PlannedHours = workSchedules
            .Where(ws => ws.EmployeeId == e.EmployeeId)
            .Sum(ws => (decimal)(ws.EndTime - ws.StartTime).TotalHours),
        WorkedTimeSpan = workShifts
            .Where(ws => ws.EmployeeId == e.EmployeeId)
            .Select(ws => ws.EndTime - ws.StartTime)
            .FirstOrDefault()
    })
    .ToList()
    .Select(e => new DayOverviewViewModel
    {
        EmployeeId = e.EmployeeId,
        FirstName = e.FirstName,
        LastName = e.LastName,
        PlannedHours = e.PlannedHours,
        WorkedHoursString = e.WorkedTimeSpan.HasValue
            ? string.Format("{0:D2}:{1:D2}", e.WorkedTimeSpan.Value.Hours, e.WorkedTimeSpan.Value.Minutes)
            : "00:00"
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

                var workShift = await _context.WorkShifts
                    .FirstOrDefaultAsync(ws => ws.EmployeeId == update.EmployeeId && ws.StartTime.Date == date.ToDateTime(TimeOnly.MinValue).Date);

                if (workShift == null)
                {

                    workShift = new WorkShift
                    {
                        EmployeeId = update.EmployeeId,
                        StartTime = date.ToDateTime(TimeOnly.MinValue),
                        EndTime = date.ToDateTime(TimeOnly.MinValue).Add(workedTimeSpan)
                    };
                    _context.WorkShifts.Add(workShift);
                }
                else
                {

                    workShift.EndTime = workShift.StartTime.Add(workedTimeSpan);
                    _context.WorkShifts.Update(workShift);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { date });
        }

    }
}

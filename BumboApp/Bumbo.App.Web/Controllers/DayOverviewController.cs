using Bumbo.App.Web.Models.ViewModels.Dayoverview;
using Bumbo.Data.Context;
using Bumbo.Domain.Services.CAO;
using Bumbo.Domain.Services.Scheduling;
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

        public async Task<IActionResult> Index(string? date)
        {
            _logger.LogInformation("open action triggered-========-==-=--=-=-------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            DateTime selectedDate;


            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out selectedDate))
            {
                selectedDate = DateTime.Today;
            }

            var employees = await _context.Employees.ToListAsync();
            var workSchedules = await _context.WorkSchedules
                .Where(ws => ws.Date == DateOnly.FromDateTime(selectedDate))
                .ToListAsync();

            var viewModel = employees.Select(e => new DayOverviewViewModel
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                PlannedHours = workSchedules
                    .Where(ws => ws.EmployeeId == e.EmployeeId)
                    .Sum(ws => (decimal)(ws.EndTime - ws.StartTime).TotalHours),
                WorkedHours = 0 /*_context.WorkShifts*/
        //.Where(ws => ws.EmployeeId == e.EmployeeId && ws.StartTime.Date == selectedDate.ToDateTime(TimeOnly.MinValue).Date)
        //.Sum(ws => (decimal)(ws.EndTime - ws.StartTime).TotalHours)
            }).ToList();


            ViewData["Date"] = DateOnly.FromDateTime(selectedDate);

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Save(List<DayOverviewViewModel> updatedHours, DateOnly date)
        {
            _logger.LogInformation("Save action triggered... -------------------------------------------------------------------------");

            //foreach (var update in updatedHours)
            //{

            //    var workShift = await _context.WorkShifts
            //        .FirstOrDefaultAsync(ws => ws.EmployeeId == update.EmployeeId && ws.StartTime.Date == date.ToDateTime(TimeOnly.MinValue).Date);

            //    if (workShift == null)
            //    {

            //        workShift = new WorkShift
            //        {
            //            EmployeeId = update.EmployeeId,
            //            StartTime = date.ToDateTime(TimeOnly.MinValue),
            //            EndTime = date.ToDateTime(TimeOnly.MinValue).AddHours((double)update.WorkedHours)
            //        };
            //        _context.WorkShifts.Add(workShift);
            //    }
            //    else
            //    {

            //        workShift.EndTime = workShift.StartTime.AddHours(update.WorkedHours);
            //        _context.WorkShifts.Update(workShift);
            //    }
            //}

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { date });
        }
    }
}

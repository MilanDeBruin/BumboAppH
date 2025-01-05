using Bumbo.Domain.Enums;
using Bumbo.App.Web.Models.ViewModels;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Bumbo.Domain.Services.Scheduling;
using Bumbo.Domain.Models.Schedueling;
using Bumbo.Domain.Services.CAO;
using Bumbo.Domain.Models;
using Bumbo.App.Web.Models.ViewModels.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace Bumbo.App.Web.Controllers;

[Authorize]
public class SchedulingController : Controller
{
    private readonly BumboDbContext _context;

    private readonly ILogger<SchedulingController> _logger;



    private Scheduling Scheduling;

    public SchedulingController(BumboDbContext context, ILogger<SchedulingController> logger, ILogger<Scheduling> _Slogger, ICaoScheduleService _caoScheduleService)
    {
        _context = context;
        _logger = logger;
        Scheduling = new Scheduling(_context, _Slogger, _caoScheduleService);
    }

    public IActionResult RoosterRefactored(int branchId, string? firstDayOfWeek)
    {
        DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
		if (firstDayOfWeek != null) date = DateOnly.Parse(firstDayOfWeek);
		DateOnly lastDateOfWeek = date.AddDays(6);

		List<EmployeeScheduleViewModel> Employees = new List<EmployeeScheduleViewModel>();

        var dbEmployees = _context.Employees.Where(e => e.BranchId == branchId).ToList();

		foreach (Employee employee in dbEmployees)
        {
            List<WorkSchedule> filteredSchedules = _context.WorkSchedules.Where(e => e.EmployeeId == employee.EmployeeId).Where(ws => ws.Date >= date && ws.Date <= lastDateOfWeek).ToList();

            for (int i = 0; i < filteredSchedules.Count; i++)
            {
                filteredSchedules[i].Department = Scheduling.getCorrectValue(filteredSchedules[i].Department);

			}

			EmployeeScheduleViewModel empData = new EmployeeScheduleViewModel
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.FirstName + " " + employee.LastName,
                // MainFunction = employee.Position, TODO: Implement using Identity
                Schedules = filteredSchedules,
            };

            Employees.Add(empData);
        }


        var forecasts = _context.Forecasts
        .Where(f => f.BranchId == branchId && f.Date >= date && f.Date <= lastDateOfWeek).ToList();


        List<ForecastData> FD = new List<ForecastData>();
        foreach (Forecast forecast in forecasts) 
        {
            string depart = Scheduling.getCorrectValue(forecast.Department);

			ForecastData data = new ForecastData();
            data.Date = forecast.Date;
            data.Department = depart;
            data.ManHours = forecast.ManHours;
            FD.Add(data);
        }
        ScheduleViewModel viewModel = new ScheduleViewModel
        {
            BranchId = branchId,
            FirstDateOfWeek = date,
            EmployeeSchedules = Employees,
            forecastDatas = FD // Add this property in your view model
        };

        return View(viewModel);
    }
    

    [HttpPost]
    public IActionResult AddSchedule(int branchId, string firstDateOfWeek, int employeeId, string date, string startTime, string endTime, string department)
    {
        ScheduleModel schedule = new ScheduleModel
        {
            EmployeeId = employeeId,
            Date = DateTime.Parse(date),
            StartTime = TimeSpan.Parse(startTime),
            EndTime = TimeSpan.Parse(endTime),
            Department = department,
        };


        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
            }
        }

        ScheduleSuccesModel result = Scheduling.SendDataToDb(schedule);
        if (result.Success)
        {
			return RedirectToAction("RoosterRefactored", "Scheduling", new { branchId = branchId, firstDayOfWeek = firstDateOfWeek });
		}
		else 
        {
			//return StatusCode(500, new { success = false, message = $"Het rooster kon niet opgeslagen worden doordat: {result.Message}" });
			return RedirectToAction("RoosterRefactored", "Scheduling", new { branchId = branchId, firstDayOfWeek = firstDateOfWeek });
		}

	}

	[HttpPost]
	public IActionResult RemoveSchedule(int branchId, string firstDateOfWeek, int employeeId, string date, string startTime, string endTime, string department)
	{
		ScheduleModel schedule = new ScheduleModel
		{
            EmployeeId = employeeId,
            Date = DateTime.Parse(date),
            StartTime = TimeSpan.Parse(startTime),
            EndTime = TimeSpan.Parse(endTime),
            Department = department,
		};

		Scheduling.DeleteDataFromDb(schedule);
		return RedirectToAction("RoosterRefactored", "Scheduling", new { branchId = branchId, firstDayOfWeek = firstDateOfWeek });
	}

	[HttpPost]
    public IActionResult PublishSchedules(int branchId, string modalDateInput)
    {
        try
        {
            DateOnly publishDate = DateOnly.Parse(modalDateInput);
            Scheduling.PublishSchedule(publishDate.AddDays(1));
            _logger.LogInformation($"{publishDate}");
            return RedirectToAction("RoosterRefactored", "Scheduling", new { branchId = branchId, firstDayOfWeek = modalDateInput });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
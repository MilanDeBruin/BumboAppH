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
using Bumbo.App.Web.Models.ViewModels.Schedule;

namespace Bumbo.App.Web.Controllers;

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

    public IActionResult Rooster()
    {

        // test data


        List<EmployeeScheduleViewModel> Employees = new List<EmployeeScheduleViewModel>();
        foreach (Employee employee in _context.Employees) 
        {
            EmployeeScheduleViewModel empData = new EmployeeScheduleViewModel
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.FirstName + " " + employee.LastName,
                MainFunction = employee.Position,
                //Schedules = employee.WorkSchedules,
            };

            Employees.Add(empData);
        }

        return View(Employees);
    }

    [HttpPost]
    public IActionResult AddSchedule([FromBody] ScheduleModel schedule)
    {

        if (schedule == null)
        {
            _logger.LogWarning("No schedule data received.");
            return BadRequest("Invalid schedule data.");
        }

       
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
            }
        }


        //_logger.LogInformation($"Received data: {schedule.EmployeeId} {schedule.Date} {schedule.StartTime} {schedule.EndTime} {schedule.Department}");
        // _logger.LogInformation($"-------------------------  {schedule.Date} --------------------------------------------------------------------------");
        CaoSheduleValidatorEnum result = Scheduling.SendDataToDb(schedule);
        if (result == CaoSheduleValidatorEnum.Valid)
        {
            return Json(new { success = true });
        }
        else 
        {
            return Json(new { success = false, message = $"Het rooster kon niet opgeslagen worden doordat: {result}" });
        }
        
    }

    [HttpPost]
    public IActionResult RemoveSchedule([FromBody] ScheduleModel schedule)
    {
        if (schedule == null)
        {
            _logger.LogWarning("No schedule data received.");
            return BadRequest("Invalid schedule data.");
        }



        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
            }
        }

        Scheduling.DeleteDataFromDb(schedule);
        return Json(new { success = true });
    }

    [HttpGet]
    public IActionResult GetSchedulesForWeek(int year, int week)
    {
        var startDate = Scheduling.GetStartOfWeek(year, week);
        var endDate = startDate.AddDays(6);

        var schedules = _context.WorkSchedules
            .Where(s => s.Date >= DateOnly.FromDateTime(startDate) && s.Date <= DateOnly.FromDateTime(endDate))
            .ToList();

        var forecasts = _context.Forecasts
            .Where(f => f.Date >= DateOnly.FromDateTime(startDate) && f.Date <= DateOnly.FromDateTime(endDate))
            .ToList();

        var employees = _context.Employees // Assuming you have an Employees table
            .Select(e => new { e.EmployeeId, Name = e.FirstName + " " + e.LastName, e.Position })
            .ToList();

        var viewModel = employees.Select(employee => new EmployeeScheduleViewModel
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            MainFunction = employee.Position,
            Schedules = schedules.Where(s => s.EmployeeId == employee.EmployeeId).Select(s => new ScheduleViewModel
            {
                EmployeeId = s.EmployeeId,
                Date = s.Date.ToString("yyyy-MM-dd"), // Properly format Date
                StartTime = s.StartTime.ToString("HH:mm"), // Properly format StartTime
                EndTime = s.EndTime.ToString("HH:mm"), // Properly format EndTime
                Department = s.Department
            }).ToList(),
            DailySummaries = Enum.GetValues(typeof(DayNameOfWeek)).Cast<DayNameOfWeek>()
                .ToDictionary(day => day, day =>
                {
                    var currentDate = startDate.AddDays((int)day); // Ensure correct mapping based on the enum value
                    var dailySchedules = schedules.Where(s => s.EmployeeId == employee.EmployeeId && s.Date == DateOnly.FromDateTime(currentDate));
                    var dailyForecast = forecasts.FirstOrDefault(f => f.Date == DateOnly.FromDateTime(currentDate));

                    return new DaySummary
                    {
                        ForecastHours = dailyForecast?.ManHours ?? 0,
                        ScheduledHours = dailySchedules.Sum(s => (s.EndTime - s.StartTime).TotalHours)
                    };
                })
        }).ToList();

        return Json(viewModel);
    }

    [HttpPost]
    public IActionResult PublishSchedules([FromBody] PublishDataModel data)
    {
        if (data == null || string.IsNullOrWhiteSpace(data.Date))
        {
            return BadRequest(new { success = false, message = "Invalid date provided." });
        }

        try
        {
            DateOnly publishDate = DateOnly.Parse(data.Date);
            Scheduling.PublishSchedule(publishDate.AddDays(1));
            _logger.LogInformation($"{publishDate}");
            return Ok(new { success = true, message = "Schedules published successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
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


        ScheduleSuccesModel result = Scheduling.SendDataToDb(schedule);
        if (result.Success)
        {
            _logger.LogInformation($"------------------------- het hoort gelukt te zijn  {schedule.Date} --------------------------------------------------------------------------");
            return Json(new { success = true });
        }
        else 
        {
            _logger.LogInformation($"------------------------- het hoort gefaalt te zijn  {schedule.Date} --------------------------------------------------------------------------");

            return Json(new { success = false, message = $"Het rooster kon niet opgeslagen worden doordat: {result.Message}" });
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
            .Select(s => new
            {
                s.EmployeeId,
                s.Date,
                s.StartTime,
                s.EndTime,
                Department = s.Department.ToString()
            })
            .ToList();

        var forecastData = _context.Forecasts
        .Where(f => f.Date >= DateOnly.FromDateTime(startDate) && f.Date <= DateOnly.FromDateTime(endDate) && f.BranchId == 1)
        .GroupBy(f => new { f.Date, f.Department })
        .Select(g => new
        {
            Date = g.Key.Date,
            Department = g.Key.Department,
            ManHours = g.Sum(f => f.ManHours) 
        })
        .ToList();

        var scheduledHours = schedules
        .GroupBy(s => new { s.Date, s.Department })
        .Select(g => new
        {
            Date = g.Key.Date,
            Department = g.Key.Department,
            ScheduledHours = g.Sum(s => (s.EndTime - s.StartTime).TotalHours) // Calculate hours
        })
        .ToList();

        return Json(new
        {
            Schedules = schedules.Select(s => new
            {
                s.EmployeeId,
                Date = s.Date.ToString("yyyy-MM-dd"),
                StartTime = s.StartTime.ToString("HH:mm"),
                EndTime = s.EndTime.ToString("HH:mm"),
                Department = s.Department
            }),
            Forecasts = forecastData.Select(f => new
            {
                Date = f.Date.ToString("yyyy-MM-dd"),
                Department = f.Department,
                ManHours = f.ManHours
            }),
            ScheduledHours = scheduledHours.Select(sh => new
            {
                Date = sh.Date.ToString("yyyy-MM-dd"),
                Department = sh.Department,
                ScheduledHours = sh.ScheduledHours
            })
        });

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
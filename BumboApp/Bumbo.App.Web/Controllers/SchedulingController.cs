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

namespace Bumbo.App.Web.Controllers;

public class SchedulingController : Controller
{
    private readonly BumboDbContext _context;

    private readonly ILogger<SchedulingController> _logger;


    private Scheduling Scheduling;

    public SchedulingController(BumboDbContext context, ILogger<SchedulingController> logger, ILogger<Scheduling> _Slogger)
    {
        _context = context;
        _logger = logger;
        Scheduling = new Scheduling(_context, _Slogger);
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
                Schedules = employee.WorkSchedules,
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
        Scheduling.SendDataToDb(schedule);
        return Json(new { success = true });
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
        return Json(schedules.Select(s => new
        {
            s.EmployeeId,
            Date = s.Date.ToString("yyyy-MM-dd"),
            StartTime = s.StartTime.ToString("HH:mm"),
            EndTime = s.EndTime.ToString("HH:mm"),
            Department = s.Department
        }));
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
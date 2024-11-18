using Bumbo.Domain.Enums;
using Bumbo.App.Web.Models.ViewModels;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Bumbo.App.Web.Controllers
{
    public class SchedulingController : Controller
    {
        private readonly BumboDbContext _context;

        private readonly ILogger<SchedulingController> _logger;

        public SchedulingController(BumboDbContext context, ILogger<SchedulingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Rooster()
        {

            // test data
            var mockEmployees = new List<EmployeeScheduleViewModel>
            {
                new EmployeeScheduleViewModel
                {
                    EmployeeId = 1,
                    MainFunction = "dkw",
                    Name = "Henk",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Department = DepartmentEnum.Shelf }
                    }
                },
                new EmployeeScheduleViewModel
                {
                    EmployeeId = 2,
                    Name = "piet",
                    MainFunction = "vers",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), Department = DepartmentEnum.Vers }
                    }
                },
                new EmployeeScheduleViewModel
                {
                    EmployeeId = 3,
                    Name = "Bob",
                    MainFunction = "Kassa",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(16), Department = DepartmentEnum.Kassa }
                    }
                }
            };

            return View(mockEmployees);
        }

        public class ScheduleModel
        {
            [Required]
            public int EmployeeId { get; set; }

            [Required]
            public string Day { get; set; }

            [Required]
            public DateTime Date { get; set; }

            [Required]
            public TimeSpan StartTime { get; set; }

            [Required]
            public TimeSpan EndTime { get; set; }

            [Required]
            public string Department { get; set; }
        }

        [HttpPost]
        public IActionResult AddSchedule([FromBody] ScheduleModel schedule)
        {

            if (schedule == null)
            {
                _logger.LogWarning("No schedule data received.");
                return BadRequest("Invalid schedule data.");
            }

            // Log model state errors
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
                }
            }

            // Log the received data
            _logger.LogInformation($"Received data: {schedule.EmployeeId} {schedule.Date} {schedule.StartTime} {schedule.EndTime} {schedule.Department}");

            // Parse the department to ensure it's valid
            if (Enum.TryParse(schedule.Department, out DepartmentEnum parsedDepartment))
            {
                var scheduleToSave = new
                {
                    EmployeeId = schedule.EmployeeId,
                    Date = schedule.Date,
                    Day = schedule.Day,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    Department = parsedDepartment
                };

                _logger.LogInformation($"Data to send to DB: {scheduleToSave}");

                // You can save scheduleToSave to your database here

                return Json(new { success = true });
            }
            else
            {
                _logger.LogWarning($"Invalid department: {schedule.Department}");
                return BadRequest("Invalid department.");
            }


        }


    }
}
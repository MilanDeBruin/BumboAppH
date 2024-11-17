using Bumbo.App.Web.Models.ViewModels;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
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
                    Name = "Henk",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Department = BumboApp.Models.Models.Department.Shelf }
                    }
                },
                new EmployeeScheduleViewModel
                {
                    EmployeeId = 2,
                    Name = "piet",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), Department = BumboApp.Models.Models.Department.Vers }
                    }
                },
                new EmployeeScheduleViewModel
                {
                    EmployeeId = 3,
                    Name = "Bob",
                    Schedules = new List<Schedule>
                    {
                        new Schedule { Date = DateTime.Now, StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(16), Department = BumboApp.Models.Models.Department.Kassa }
                    }
                }
            };

            return View(mockEmployees);
        }

        public class ScheduleModel
        {
            public int EmployeeId { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
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

            // Log the received data
            _logger.LogInformation($"Received data: {schedule.EmployeeId} {schedule.Date} {schedule.StartTime} {schedule.EndTime} {schedule.Department}");

            // Parse the department to ensure it's valid
            if (Enum.TryParse(schedule.Department, out BumboApp.Models.Models.Department parsedDepartment))
            {
                var scheduleToSave = new
                {
                    EmployeeId = schedule.EmployeeId,
                    Date = schedule.Date,
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
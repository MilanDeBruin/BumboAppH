using Bumbo.App.Web.Models.ViewModels.Availability;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class AvailabilityController(BumboDbContext context) : Controller
    {
        private readonly BumboDbContext _context = context;

        public IActionResult Index(int? employeeId)
        {
            employeeId = 1;

            var availabilities = _context.Availabilities
                .Where(a => a.EmployeeId == employeeId)
                .GroupBy(a => a.EmployeeId)
                .Select(g => new AvailabilityViewModel
                {
                    EmployeeId = g.Key,
                    DailyAvailabilities = g.Select(a => new DailyAvailability
                    {
                        Weekday = a.Weekday,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime
                    }).ToList()
                })
                .FirstOrDefault();

            if (availabilities == null) return View(new List<AvailabilityViewModel>());

            return View(new List<AvailabilityViewModel> { availabilities });
        }

        [HttpGet]
        public IActionResult Edit(int employeeId)
        {
            var weekdays = _context.Weekdays.Select(w => w.Weekday1).ToList();
            var existingAvailabilities = _context.Availabilities.Where(a => a.EmployeeId == employeeId).ToList();

            var availabilityViewModel = new AvailabilityViewModel
            {
                EmployeeId = employeeId,
                DailyAvailabilities = weekdays.Select(day =>
                {
                    var availability = existingAvailabilities.FirstOrDefault(a => a.Weekday == day);
                    
                    return new DailyAvailability
                    {
                        Weekday = day,
                        StartTime = availability?.StartTime,
                        EndTime = availability?.EndTime
                    };
                }).ToList()
            };

            return View(availabilityViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AvailabilityViewModel availabilityViewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var dailyAvailability in availabilityViewModel.DailyAvailabilities)
                {
                    var availability = _context.Availabilities
                        .FirstOrDefault(a => a.EmployeeId == availabilityViewModel.EmployeeId && a.Weekday == dailyAvailability.Weekday);

                    if (dailyAvailability.StartTime.HasValue && dailyAvailability.EndTime.HasValue)
                    {
                        if (availability == null)
                        {
                            availability = new Availability
                            {
                                EmployeeId = availabilityViewModel.EmployeeId,
                                Weekday = dailyAvailability.Weekday,
                                StartTime = dailyAvailability.StartTime,
                                EndTime = dailyAvailability.EndTime
                            };
                            _context.Availabilities.Add(availability);
                        }
                        else
                        {
                            availability.StartTime = dailyAvailability.StartTime;
                            availability.EndTime = dailyAvailability.EndTime;
                        }
                    }
                    else if (availability != null) _context.Availabilities.Remove(availability);
                }
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Beschikbaarheid is bijgewerkt!";
                return RedirectToAction("Index");
            }

            return View(availabilityViewModel);
        }
    }
}

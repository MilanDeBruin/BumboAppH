using Bumbo.App.Web.Models.ViewModels.Availability;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bumbo.App.Web.Models.ViewModels.Branch;
using Bumbo.Data.Interfaces;
using Bumbo.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Bumbo.App.Web.Models.ViewModels.Employee;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class AvailabilityController(BumboDbContext context, IAvailabilityRepository availabilityRepository) : Controller
    {
        private readonly IAvailabilityRepository _availabilityRepository = availabilityRepository;
        private readonly BumboDbContext _context = context;

        public IActionResult Details(int employeeId, int branchId, string? firstDayOfWeek)
        {
            DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            if (firstDayOfWeek != null) date = DateOnly.Parse(firstDayOfWeek);

            List<Availability> weekAvailability =
                _availabilityRepository.GetWeekAvailabilityForEmployee(employeeId, date).OrderBy(f => f.Weekday).ToList();

            AvailabilityViewModel viewModel = new AvailabilityViewModel
            {
                EmployeeId = employeeId,
                BranchId = branchId,
                DailyAvailabilities = new List<DailyAvailability>()
            };

            if (weekAvailability.IsNullOrEmpty()) return View(viewModel);

            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = date.AddDays(i);
                var dailyAvailability = new DailyAvailability
                {
                    Weekday = dayDate,
                    StartTime = null,
                    EndTime = null,
                };

                var employeeAvailability = weekAvailability.FirstOrDefault(a =>
                    a.Weekday.ToLower() == dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")).ToLower());

                if (employeeAvailability != null)
                {
                    dailyAvailability.StartTime = employeeAvailability.StartTime;
                    dailyAvailability.EndTime = employeeAvailability.EndTime;
                }

                viewModel.DailyAvailabilities.Add(dailyAvailability);
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int employeeId, int branchId)
        {
            DateOnly startDate = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));

            var existingAvailabilities = _context.Availabilities
                .Where(a => a.EmployeeId == employeeId)
                .ToList();

            var availabilityViewModel = new AvailabilityViewModel
            {
                EmployeeId = employeeId,
                BranchId = branchId,
                DailyAvailabilities = new List<DailyAvailability>()
            };

            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = startDate.AddDays(i);

                var dailyAvailability = new DailyAvailability
                {
                    Weekday = dayDate,
                    StartTime = null,
                    EndTime = null
                };

                var employeeAvailability = existingAvailabilities.FirstOrDefault(a =>
                    a.Weekday.ToLower() == dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")).ToLower());

                if (employeeAvailability != null)
                {
                    dailyAvailability.StartTime = employeeAvailability.StartTime;
                    dailyAvailability.EndTime = employeeAvailability.EndTime;
                }

                availabilityViewModel.DailyAvailabilities.Add(dailyAvailability);
            }

            return View(availabilityViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AvailabilityViewModel availabilityViewModel)
        {
            if (!ModelState.IsValid) return View(availabilityViewModel);

            // Door iedere werkdag van het formulier lopen en de index van de huidige loop bijhouden
            foreach (var (dailyAvailability, i) in availabilityViewModel.DailyAvailabilities.Select((value, index) => (value, index)))
            {
                string weekdayName = dailyAvailability.Weekday.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")).ToLower();
                var availability = _context.Availabilities.FirstOrDefault(a => a.EmployeeId == availabilityViewModel.EmployeeId && a.Weekday.ToLower() == weekdayName);
                var storeOpeningHour = _availabilityRepository.GetStoreOpeningHour(availabilityViewModel.BranchId, dailyAvailability.Weekday);
                var storeClosingHour = _availabilityRepository.GetStoreClosingHour(availabilityViewModel.BranchId, dailyAvailability.Weekday);

                if (dailyAvailability.StartTime.HasValue && dailyAvailability.EndTime.HasValue)
                {
                    if (dailyAvailability.StartTime < storeOpeningHour || dailyAvailability.EndTime > storeClosingHour)
                    {
                        ModelState.AddModelError(
                            $"DailyAvailabilities[{i}].StartTime",
                            $"Beschikbaarheid moet binnen de openingstijden vallen ({storeOpeningHour} - {storeClosingHour})"
                        );
                        ModelState.AddModelError(
                            $"DailyAvailabilities[{i}].EndTime",
                            $"Beschikbaarheid moet binnen de openingstijden vallen ({storeOpeningHour} - {storeClosingHour})"
                        );
                        return View(availabilityViewModel);
                    }

                    if (availability == null)
                    {
                        _context.Availabilities.Add(new Availability
                        {
                            EmployeeId = availabilityViewModel.EmployeeId,
                            Weekday = weekdayName,
                            StartTime = dailyAvailability.StartTime.Value,
                            EndTime = dailyAvailability.EndTime.Value
                        });
                    }
                    else
                    {
                        availability.StartTime = dailyAvailability.StartTime.Value;
                        availability.EndTime = dailyAvailability.EndTime.Value;
                    }
                }
                else if (availability != null) _context.Availabilities.Remove(availability);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Beschikbaarheid succesvol bijgewerkt!";
            return RedirectToAction("Details", new { employeeId = availabilityViewModel.EmployeeId, branchId = availabilityViewModel.BranchId });
        }

        [HttpGet]
        public IActionResult Index(int branchId, string? firstDayOfWeek, string? position)
        {
            DateOnly date = DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            if (firstDayOfWeek != null)
            {
                date = DateOnly.Parse(firstDayOfWeek);
            }

            // Haal de beschikbaarheden voor de week op
            List<Availability> weekAvailabilities =
                _availabilityRepository.GetWeekAvailability(branchId, date).OrderBy(f => f.Weekday).ToList();

            // Maak een nieuw viewmodel
            WeekAvailabilityViewModel viewModel = new WeekAvailabilityViewModel
            {
                BranchId = branchId,
                FirstDayOfWeek = date,
                DayAvailabilities = new List<DayAvailabilityViewModel>()
            };

            // Als er geen beschikbaarheden zijn, stuur het lege viewmodel terug
            if (weekAvailabilities.IsNullOrEmpty())
            {
                return View(viewModel);
            }

            BranchWeekOpeningTimeViewModel branchWeekOpeningClosingTime = new BranchWeekOpeningTimeViewModel
            {
                BranchId = branchId
            };

            // Voeg voor elke dag de beschikbaarheden van medewerkers toe
            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = date.AddDays(i);
                var dayAvailability = new DayAvailabilityViewModel
                {
                    WeekDay = dayDate,
                    EmployeeAvailabilities = new List<EmployeeAvailability>()
                };
                BranchDayOpeningTimeViewModel branchOpeningClosingTime = new BranchDayOpeningTimeViewModel
                {
                    BranchId = branchId,
                    Day = dayDate,
                    StartTime = _availabilityRepository.GetStoreOpeningHour(branchId, dayDate),
                    EndTime = _availabilityRepository.GetStoreClosingHour(branchId, dayDate)
                };
                branchWeekOpeningClosingTime.OpeningTimes.Add(branchOpeningClosingTime);

                // Filter beschikbaarheden op de geselecteerde positie, indien aanwezig
                var filteredAvailabilities = weekAvailabilities.Where(a =>
                    a.Weekday.ToLower() == dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")).ToLower() &&
                    (string.IsNullOrEmpty(position) /*|| a.Employee.Position == position TODO: Implement using Identity*/)).ToList(); 

                foreach (var availability in filteredAvailabilities)
                {
                    var employee = availability.Employee;
                    dayAvailability.EmployeeAvailabilities.Add(new EmployeeAvailability
                    {
                        Employee = new EmployeeViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            BranchId = employee.BranchId,
                            // Position = employee.Position, TODO: Implement using Identity
                            HiringDate = employee.HiringDate,
                            FirstName = employee.FirstName,
                            Infix = employee.Infix,
                            LastName = employee.LastName,
                            DateOfBirth = employee.DateOfBirth,
                            HouseNumber = employee.HouseNumber,
                            Addition = employee.Addition,
                            ZipCode = employee.ZipCode,
                            // EmailAdres = employee.EmailAdres, TODO: Implement using Identity
                            // Password = employee.Password, TODO: Implement using Identity
                        },
                        StartTime = availability.StartTime,
                        EndTime = availability.EndTime,
                    });
                }

                viewModel.DayAvailabilities.Add(dayAvailability);
            }

            viewModel.OpeningDurations = branchWeekOpeningClosingTime;
            return View(viewModel);
        }

    }
}

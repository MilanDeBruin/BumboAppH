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
using Microsoft.VisualBasic;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class AvailabilityController : Controller
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly BumboDbContext _context;

        public AvailabilityController(BumboDbContext context, IAvailabilityRepository availabilityRepository)
        {
            this._availabilityRepository = availabilityRepository;
            this._context = context;
        }

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
                    EndTime = null
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
                else if (availability != null)
                {
                    _context.Availabilities.Remove(availability);
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Beschikbaarheid succesvol bijgewerkt!";
            return RedirectToAction("Details", new { employeeId = availabilityViewModel.EmployeeId, branchId = availabilityViewModel.BranchId });
        }



        [HttpGet]
        public IActionResult Index(int branchId, string? firstDayOfWeek)
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

            BranchWeekOpeningTimeViewModel branchWeekOpeningClosingTime = new BranchWeekOpeningTimeViewModel();
            branchWeekOpeningClosingTime.BranchId = branchId;

            // Voeg voor elke dag de beschikbaarheden van medewerkers toe
            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = date.AddDays(i);
                var dayAvailability = new DayAvailabilityViewModel
                {
                    WeekDay = dayDate,
                    EmployeeAvailabilities = new List<EmployeeAvailability>()
                };
                BranchDayOpeningTimeViewModel branchOpeningClosingTime = new BranchDayOpeningTimeViewModel()
                {
                    BranchId = branchId,
                    Day = dayDate,
                    StartTime = _availabilityRepository.GetStoreOpeningHour(branchId, date),
                    EndTime = _availabilityRepository.GetStoreClosingHour(branchId, date)
                };
                branchWeekOpeningClosingTime.OpeningTimes.Add(branchOpeningClosingTime);

                // Voeg per dag de medewerkers beschikbaarheden toe (voorbeeld, je zou dit verder moeten aanpassen)
                foreach (var availability in weekAvailabilities.Where(a => a.Weekday.ToLower() == dayDate.ToString("dddd", new System.Globalization.CultureInfo("nl-NL")).ToLower()))
                {
                    var employee = availability.Employee;
                    dayAvailability.EmployeeAvailabilities.Add(new EmployeeAvailability
                    {
                        Employee = new EmployeeViewModel()
                        {
                            employee_id = employee.EmployeeId,
                            branch_id = employee.BranchId,
                            position = employee.Position,
                            hiring_date = employee.HiringDate,
                            first_name = employee.FirstName,
                            infix = employee.Infix,
                            last_name = employee.LastName,
                            date_of_birth = employee.DateOfBirth,
                            house_number = employee.HouseNumber,
                            addition = employee.Addition,
                            zip_code = employee.ZipCode,
                            email_adres = employee.EmailAdres,
                            password = employee.Password,
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

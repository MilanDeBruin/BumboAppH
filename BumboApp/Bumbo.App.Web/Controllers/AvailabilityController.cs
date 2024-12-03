using Bumbo.App.Web.Models.ViewModels.Availability;
using Bumbo.App.Web.Models.ViewModels.Branch;
using Bumbo.App.Web.Models.ViewModels.Employee;
using Bumbo.App.Web.Models.ViewModels.Forecast;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]

    public class AvailabilityController : Controller
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public AvailabilityController(BumboDbContext context, IAvailabilityRepository availabilityRepository)
        {
            this._availabilityRepository = availabilityRepository;
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
                        Employee = new EmployeeModel()
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

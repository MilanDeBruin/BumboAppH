using Bumbo.App.Web.Models.Enums;
using Bumbo.App.Web.Models.Models;
using Bumbo.App.Web.Models.Models.Forecast;
using Bumbo.App.Web.Models.Repositorys;
using Bumbo.App.Web.Models.ViewModels.Forecast;
using Bumbo.Data.Context;
using Bumbo.Data.External;
using Bumbo.Data.Models;
using BumboApp.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BumboApp.Controllers
{
    [Authorize]
    public class ForecastController : Controller
    {
        private readonly BumboDbContext _dbContext;
        
        public ForecastController(BumboDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(int branchId, DateOnly? firstDayOfWeek)
        {
            DateOnly date = firstDayOfWeek ?? DateOnlyHelper.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Now));
            ForecastRepository forecastRepository = new ForecastRepository(_dbContext);
            List<Forecast> weekForecasts =
                forecastRepository.GetWeekForecast(branchId, date).OrderBy(f => f.Date).ToList();
            WeekForecastViewModel viewModel = new WeekForecastViewModel();
            viewModel.BranchId = branchId;
            viewModel.FirstDayOfWeek = date;

            if (weekForecasts.IsNullOrEmpty())
            {
                return View(viewModel);
            }

            viewModel.DayForecasts = new List<DayForecastViewModel>();
            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = date.AddDays(i);
                viewModel.DayForecasts.Add(new DayForecastViewModel()
                {
                    Date = dayDate,
                    CheckoutHours = weekForecasts.Find(f => f.Date == dayDate && f.Department == "Kassa").ManHours,
                    FreshHours = weekForecasts.Find(f => f.Date == dayDate && f.Department == "Vers").ManHours,
                    ShelfStackerHours = weekForecasts.Find(f => f.Date == dayDate && f.Department == "Shelf").ManHours
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GenerateForecast(int branchId, DateOnly firstDayOfWeek)
        {
            HolidayRepository holidayRepository = new HolidayRepository();
            GenerateForecastViewModel viewModel = new GenerateForecastViewModel()
            {
                BranchId = branchId,
                FirstDateOfWeek = firstDayOfWeek,
                NextHoliday = holidayRepository.GetHoliday(branchId, firstDayOfWeek),
                Days = new List<GenerateForecastDayViewModel>()
            };

            WeatherRepository weatherRepository = new WeatherRepository();
            List<WeatherDayModel>? weather = weatherRepository.GetWeather(branchId, firstDayOfWeek);

            for (int i = 0; i < 7; i++)
            {
                DateOnly dayDate = firstDayOfWeek.AddDays(i);
                double? dayTemp = null;
                int dayCustomers = 1000;
                int dayCollies = 0;
                if (weather != null)
                {
                    dayTemp = weather[i].MaxTemp;
                }

                viewModel.Days.Add(new GenerateForecastDayViewModel()
                {
                    Date = dayDate,
                    AmountOfCustomers = dayCustomers,
                    AmountOfCollies = dayCollies,
                    Temperature = dayTemp
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult GenerateForecast(GenerateForecastViewModel viewModel)
        {
            List<GenerateForecastDayModel> dayModels = new List<GenerateForecastDayModel>();
            foreach (var day in viewModel.Days)
            {
                dayModels.Add(new GenerateForecastDayModel(day.Date, day.AmountOfCustomers, day.AmountOfCollies,
                    day.Temperature));
            }

            GenerateForecastModel forecastModel =
                new GenerateForecastModel(viewModel.BranchId, viewModel.NextHoliday, dayModels, _dbContext);
            forecastModel.GenerateForecast();

            return RedirectToAction("Index", "Forecast", new
            {
                branchId = viewModel.BranchId,
                firstDayOfWeek = viewModel.FirstDateOfWeek
            });
        }

        [HttpPost]
        public IActionResult UpdateForecast(WeekForecastViewModel viewModel)
        {
            ForecastRepository repository = new ForecastRepository(_dbContext);
            List<Forecast> updatedForecasts = new List<Forecast>();

            foreach (var dayForecast in viewModel.DayForecasts)
            {
                updatedForecasts.Add(new Forecast()
                {
                    BranchId = viewModel.BranchId,
                    Date = dayForecast.Date,
                    Department = DepartmentEnum.Kassa.ToString(),
                    ManHours = dayForecast.CheckoutHours
                });
                updatedForecasts.Add(new Forecast()
                {
                    BranchId = viewModel.BranchId,
                    Date = dayForecast.Date,
                    Department = DepartmentEnum.Vers.ToString(),
                    ManHours = dayForecast.FreshHours
                });
                updatedForecasts.Add(new Forecast()
                {
                    BranchId = viewModel.BranchId,
                    Date = dayForecast.Date,
                    Department = DepartmentEnum.Shelf.ToString(),
                    ManHours = dayForecast.ShelfStackerHours
                });
            }

            repository.SetWeekForecast(updatedForecasts);

            return RedirectToAction("Index", new
            {
                branchid = viewModel.BranchId,
                firstDayOfWeek = viewModel.FirstDayOfWeek
            });
        }
    }
}
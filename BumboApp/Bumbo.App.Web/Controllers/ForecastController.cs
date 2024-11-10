using Bumbo.App.Web.Models.Services;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BumboApp.Controllers
{
    [Authorize]
    public class ForecastController : Controller
    {
        private readonly ILogger<ForecastGenerator> _logger;
        private readonly ILogger<ForecastController> _loggerCon;

        private readonly ForecastGenerator forecastGenerator;

       
        public ForecastController(ILogger<ForecastGenerator> logger, ILogger<ForecastController> loggerCon,BumboDbContext context)
        {
            _logger = logger;
            _loggerCon = loggerCon;
            this.forecastGenerator = new ForecastGenerator(_logger, context);
        }

        [HttpGet]
        public IActionResult GeneratePrognose(int weekNumber, int year)
        {
            WeekInputViewModel viewModel = new WeekInputViewModel
            {
                WeekNumber = weekNumber,
                Year = year,
                DayInputs = new List<DayData>
                {
                    new DayData() { DayName = "Maandag",Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Dinsdag" ,Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Woensdag",Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Donderdag",Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Vrijdag",Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Zaterdag",Collies = 0,ExpectedCustomers = 0 },
                    new DayData { DayName = "Zondag",Collies = 0,ExpectedCustomers = 0 }
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitWeekInput(WeekInputViewModel viewModel)
        {
            int branchId = 1;
            
            DateOnly firstDayOfWeek = FirstDateOfWeek(viewModel.WeekNumber, viewModel.Year);
            StoreTrafficEstimationService storeTrafficEstimationService = new StoreTrafficEstimationService();
            List<StoreTraffic> estimatedStoreTraffic;
            try
            {
                estimatedStoreTraffic =
                    storeTrafficEstimationService.EstimateStoreTraffic(branchId, firstDayOfWeek);
            }
            catch (Exception e)
            {
                return RedirectToAction("GeneratePrognoseError");
            }

            List<DayData> dayDataList = new List<DayData>();

            for (int i = 0; i < 7; i++)
            {
                _loggerCon.LogInformation("voeg data toe");
                DayData data = new DayData();
                data.ExpectedCustomers = estimatedStoreTraffic[i].Amount;
                data.Collies = viewModel.DayInputs[i].Collies;
                dayDataList.Add(data);
            }
            forecastGenerator.GenerateAllForecasts(dayDataList, viewModel.WeekNumber, firstDayOfWeek, branchId);

            
            return RedirectToAction("prognose", "Home",new { weekNumber = viewModel.WeekNumber, year = viewModel.Year });
        }


        [Route("Home/Prognose/{weekNumber:int?}")]
        public ActionResult Prognose(int? weekNumber)
        {
            DateTime today = DateTime.Now;
            var year = today.Year;
            int currentWeek = this.GetWeekNumber(today);

            int selectedWeek = weekNumber ?? currentWeek;

            if (selectedWeek < 1)
            {
                selectedWeek = 1;
            }
            if (selectedWeek > 52)
            {
                selectedWeek = 52;
            }

            ViewBag.SelectedWeek = selectedWeek;
            ViewBag.CurrentYear = today.Year;
            ViewBag.WeekForecast = this.GetWeekForecast(selectedWeek, year);

            return View();
        }

        [HttpGet]
        public IActionResult GeneratePrognoseError()
        {
            return View();
        }

        private int GetWeekNumber(DateTime date)
        {
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            return cul.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private DateOnly FirstDateOfWeek(int weekNumber, int year)
        {
            var jan1 = new DateOnly(year, 1, 1);
            var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            var firstMonday = jan1.AddDays(daysOffset >= 0 ? daysOffset : daysOffset + 7);
            var day = firstMonday.AddDays((weekNumber - 1) * 7);
            day = new DateOnly(year, day.Month, day.Day);
            return day;
        }


        private WeekForecast GetWeekForecast(int weekNumber, int year)
        {
            List<DateOnly> dates = new List<DateOnly>();
            for (var i = 0; i < 7; i++)
            {
                dates.Add(this.FirstDateOfWeek(weekNumber, year).AddDays(i));
            }

            WeekForecast weekForecast = this.forecastGenerator.GetWeekForecast(dates);

            return weekForecast;
        }
    }
}

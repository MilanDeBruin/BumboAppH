using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Controllers;

namespace BumboApp.Models.Models
{
    public class ForecastGenerator
    {
        private readonly ILogger<ForecastGenerator> _logger;
        private readonly BumboDbContext dbContext;
        private readonly float verwachtGewerkteUur = 8;

        public ForecastGenerator(ILogger<ForecastGenerator> logger, BumboDbContext context)
        {
            _logger = logger;
            dbContext = context;
        }

        public void GenerateAllForecasts(List<DayData> dayData, int weekNumber, DateOnly firstDayOftheWeek, int branchID)
        {
            WeekForecast week = new WeekForecast { WeekNumber = weekNumber };

            _logger.LogInformation("dayData.Count: " + dayData.Count);
            for (int i = 0; i < dayData.Count; i++)
            {
                DateOnly date = firstDayOftheWeek.AddDays(i);
                DayForecast day = new DayForecast(date, branchID);

                day.AddNewForecast(Department.Shelf, (int)Math.Ceiling(GenerateShelf(dayData[i].Collies, branchID)));
                day.AddNewForecast(Department.Vers, (int)Math.Ceiling(GenerateFresh(dayData[i].ExpectedCustomers, branchID)));
                day.AddNewForecast(Department.Kassa, (int)Math.Ceiling(GenerateRegister(dayData[i].ExpectedCustomers, branchID)));

                week.WeekList.Add(day);
                _logger.LogInformation("Added a day forecast to the week.");
            }
            SendToDB(week);
        }

        public float GenerateShelf(int collies, int branchID)
        {
            var uitladenNormeringen = dbContext.Norms.FirstOrDefault(f => f.BranchId == branchID && f.SupermarketActivity == "Coli uitladen");
            var vakkenVullenNormeringen = dbContext.Norms.FirstOrDefault(f => f.BranchId == branchID && f.SupermarketActivity == "Vakken vullen");

            if (uitladenNormeringen == null || vakkenVullenNormeringen == null)
                throw new InvalidOperationException("Required norm not found for Shelf activity.");

            float shelfStackInHour = collies / (float)vakkenVullenNormeringen.NormPerHour;
            float colliUnloadInHour = collies / (float)uitladenNormeringen.NormPerHour;
            return shelfStackInHour + colliUnloadInHour;
        }

        public float GenerateRegister(int customers, int branchID)
        {
            var normeringen = dbContext.Norms.FirstOrDefault(f => f.BranchId == branchID && f.SupermarketActivity == "Kassa");
            if (normeringen == null)
                throw new InvalidOperationException("No matching norm found for Register activity.");

            return customers / (float)normeringen.NormPerHour;
        }

        public float GenerateFresh(int customers, int branchID)
        {
            var normeringen = dbContext.Norms.FirstOrDefault(f => f.BranchId == branchID && f.SupermarketActivity == "Vers");
            if (normeringen == null)
                throw new InvalidOperationException("No matching norm found for Fresh activity.");

            return customers / (float)normeringen.NormPerHour;
        }

        public WeekForecast GetWeekForecast(List<DateOnly> weekDates)
        {
            var forecasts = dbContext.Forecasts
                .Where(f => weekDates.Contains(f.Date))
                .ToList();

            WeekForecast weekForecast = new WeekForecast();
            var forecastsByDate = forecasts.GroupBy(f => f.Date).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var date in weekDates)
            {
                if (forecastsByDate.TryGetValue(date, out var dayForecasts) && dayForecasts.Any())
                {
                    var dayForecast = new DayForecast(date, dayForecasts[0].BranchId) { forecasts = dayForecasts };
                    weekForecast.WeekList.Add(dayForecast);
                }
            }
            return weekForecast;
        }

        private void SendToDB(WeekForecast week)
        {
            _logger.LogInformation("Total number of days to process: " + week.WeekList.Count);

            foreach (var day in week.WeekList)
            {
                foreach (var item in day.forecasts)
                {
                    var existingForecast = dbContext.Forecasts
                        .FirstOrDefault(f => f.Date == item.Date
                                             && f.BranchId == item.BranchId
                                             && f.Department == item.Department);

                    if (existingForecast != null)
                    {
                        existingForecast.ManHours = item.ManHours;
                        _logger.LogInformation($"Updated forecast for Date: {item.Date}, BranchId: {item.BranchId}, Department: {item.Department}.");
                    }
                    else
                    {
                        dbContext.Forecasts.Add(item);
                        _logger.LogInformation($"Added new forecast for Date: {item.Date}, BranchId: {item.BranchId}, Department: {item.Department}.");
                    }
                }
            }

            dbContext.SaveChanges();
        }
    }
}

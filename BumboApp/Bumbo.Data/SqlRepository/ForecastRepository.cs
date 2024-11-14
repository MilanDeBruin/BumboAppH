using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class ForecastRepository : IForecastRepository
{
    private readonly BumboDbContext _dbContext;

    public ForecastRepository(BumboDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public List<Forecast> GetWeekForecast(int branchId, DateOnly firstDayOfWeek)
    {
        List<DateOnly> datesOfWeek = new List<DateOnly>();
        for (int i = 0; i < 7; i++)
        {
            datesOfWeek.Add(firstDayOfWeek.AddDays(i));
        }

        List<Forecast> weekForecasts = _dbContext.Forecasts.Where(f => datesOfWeek.Contains(f.Date) && f.BranchId == branchId).ToList();

        return weekForecasts;
    }

    public void SetWeekForecast(List<Forecast> forecasts)
    {
        foreach (var forecast in forecasts)
        {
            Forecast? currentForecast = _dbContext.Forecasts.FirstOrDefault(f =>
                f.BranchId == forecast.BranchId && 
                f.Date == forecast.Date && 
                f.Department == forecast.Department
                );

            if (currentForecast == null)
            {
                _dbContext.Forecasts.Add(forecast);
                continue;
            }

            currentForecast.ManHours = forecast.ManHours;
        }

        _dbContext.SaveChanges();
    }
    
    
}
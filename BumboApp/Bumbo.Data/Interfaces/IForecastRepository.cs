using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IForecastRepository
{
    public List<Forecast> GetWeekForecast(int branchId, DateOnly firstDayOfWeek);
    public void SetWeekForecast(List<Forecast> forecasts);
}
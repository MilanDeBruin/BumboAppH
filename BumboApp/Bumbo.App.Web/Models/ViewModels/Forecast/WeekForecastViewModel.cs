namespace Bumbo.App.Web.Models.ViewModels.Forecast;

public class WeekForecastViewModel
{
    public int BranchId { get; set; }
    public DateOnly FirstDayOfWeek { get; set; }
    public List<DayForecastViewModel> DayForecasts { get; set; }
}
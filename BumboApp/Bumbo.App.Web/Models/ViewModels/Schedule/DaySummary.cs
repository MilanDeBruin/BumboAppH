namespace Bumbo.App.Web.Models.ViewModels.Schedule;

public class DaySummary
{
    public DayNameOfWeek Day { get; set; }
    public double ForecastHours { get; set; }
    public double ScheduledHours { get; set; }
    public double RemainingHours => ForecastHours - ScheduledHours;
}

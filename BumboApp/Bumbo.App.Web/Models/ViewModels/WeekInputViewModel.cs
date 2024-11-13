using Bumbo.Functionality.Forecasts;
namespace Bumbo.App.Web.Models.ViewModels;

public class WeekInputViewModel
{
    public int WeekNumber { get; set; } // Current week
    public int Year { get; set; } // Current year
    public List<DayData> DayInputs { get; set; } = new List<DayData>(); // List to hold data for each day


}

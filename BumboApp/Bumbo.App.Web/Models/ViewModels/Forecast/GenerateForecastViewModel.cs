using BumboApp.Models.Models;

namespace Bumbo.App.Web.Models.ViewModels.Forecast;

public class GenerateForecastViewModel
{
    public int BranchId { get; set; }
    public DateOnly FirstDateOfWeek { get; set; }
    public HolidayModel? NextHoliday { get; set; }
    public List<GenerateForecastDayViewModel> Days { get; set; }
}
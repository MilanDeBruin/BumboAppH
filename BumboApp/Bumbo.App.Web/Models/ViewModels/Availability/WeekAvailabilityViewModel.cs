namespace Bumbo.App.Web.Models.ViewModels.Availability;
using Bumbo.App.Web.Models.ViewModels.Branch;

public class WeekAvailabilityViewModel
{
    public int BranchId { get; set; }
    public DateOnly FirstDayOfWeek { get; set; }
    public BranchWeekOpeningTimeViewModel OpeningDurations { get; set; }
    public List<DayAvailabilityViewModel> DayAvailabilities { get; set; }
}
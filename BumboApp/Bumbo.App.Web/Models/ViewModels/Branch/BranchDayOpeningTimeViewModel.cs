namespace Bumbo.App.Web.Models.ViewModels.Branch;

public class BranchDayOpeningTimeViewModel
{
    public int BranchId { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}

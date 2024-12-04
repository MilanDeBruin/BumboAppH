namespace Bumbo.App.Web.Models.ViewModels.Branch;

public class BranchWeekOpeningTimeViewModel
{
    public int BranchId { get; set; }
    public List<BranchDayOpeningTimeViewModel>? OpeningTimes { get; set; } = new List<BranchDayOpeningTimeViewModel>();
}

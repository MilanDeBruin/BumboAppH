using Bumbo.App.Web.Models.ViewModels.Dayoverview;

namespace Bumbo.App.Web.Models.ViewModels;

public class MonthlyOverviewViewModel
{
    public DateOnly Date { get; set; }
    public List<DayOverviewViewModel> Employees { get; set; }

    public bool IsApproved { get; set; }
}

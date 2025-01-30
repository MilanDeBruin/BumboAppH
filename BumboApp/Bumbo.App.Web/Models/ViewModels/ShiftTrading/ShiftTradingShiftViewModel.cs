namespace Bumbo.App.Web.Models.ViewModels.ShiftTrading;

public class ShiftTradingShiftViewModel
{
    public string EmployeeName { get; set; }
    public string ClaimEmployeeName { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Department { get; set; }
    public int BranchId { get; set; }
}
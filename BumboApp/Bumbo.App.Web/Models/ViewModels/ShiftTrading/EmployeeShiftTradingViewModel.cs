using Bumbo.Domain.Enums;

namespace Bumbo.App.Web.Models.ViewModels.ShiftTrading;

public class EmployeeShiftTradingViewModel : ShiftTradingViewModel
{
    public CaoSheduleValidatorEnum? claimStatus { get; set; }
    public int CurrentEmployeeId { get; set; }
    
    
}
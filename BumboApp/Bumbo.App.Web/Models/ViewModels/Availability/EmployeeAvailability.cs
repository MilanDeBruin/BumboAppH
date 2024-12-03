using Bumbo.App.Web.Models.ViewModels.Employee;

namespace Bumbo.App.Web.Models.ViewModels.Availability;

public class EmployeeAvailability
{
    public EmployeeModel Employee { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}

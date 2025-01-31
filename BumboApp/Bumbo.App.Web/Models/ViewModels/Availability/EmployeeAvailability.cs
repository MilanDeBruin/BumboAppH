using Bumbo.App.Web.Models.ViewModels.Employee;

namespace Bumbo.App.Web.Models.ViewModels.Availability;

public class EmployeeAvailability
{
    public EmployeeCreateViewModel EmployeeCreate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}

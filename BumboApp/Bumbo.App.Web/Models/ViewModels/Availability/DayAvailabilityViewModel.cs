namespace Bumbo.App.Web.Models.ViewModels.Availability;

public class DayAvailabilityViewModel
{
    public List<EmployeeAvailability> EmployeeAvailabilities { get; set; }
    public DateOnly WeekDay { get; set; }
   
}
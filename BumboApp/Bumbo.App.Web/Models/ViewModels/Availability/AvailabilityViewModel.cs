namespace Bumbo.App.Web.Models.ViewModels.Availability;

public class AvailabilityViewModel
{
    public int EmployeeId { get; set; }
    public List<DailyAvailability> DailyAvailabilities { get; set; } = new List<DailyAvailability>();
}

public class DailyAvailability
{
    public DateOnly Weekday { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}

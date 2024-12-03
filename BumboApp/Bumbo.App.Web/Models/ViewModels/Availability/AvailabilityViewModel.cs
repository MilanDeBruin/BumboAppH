namespace Bumbo.App.Web.Models.ViewModels.Availability
{
    public class AvailabilityViewModel
    {
        public int EmployeeId { get; set; }
        public List<DailyAvailability> DailyAvailabilities { get; set; } = new List<DailyAvailability>();
        public List<string> Weekdays { get; set; } = new List<string>();
    }

    public class DailyAvailability
    {
        public string Weekday { get; set; } = null!;
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}

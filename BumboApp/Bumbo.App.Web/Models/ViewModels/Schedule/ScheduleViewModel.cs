namespace Bumbo.App.Web.Models.ViewModels.Schedule
{
    public class ScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public string Date { get; set; } // Formatted as "yyyy-MM-dd"
        public string StartTime { get; set; } // Formatted as "HH:mm"
        public string EndTime { get; set; } // Formatted as "HH:mm"
        public string Department { get; set; }
    }
}

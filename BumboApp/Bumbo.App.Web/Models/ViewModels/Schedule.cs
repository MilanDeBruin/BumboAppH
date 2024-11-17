using BumboApp.Models.Models;
using Bumbo.App.Web.Models.ViewModels;

namespace Bumbo.App.Web.Models.ViewModels
{
    public class Schedule
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Department Department { get; set; }

        public EmployeeScheduleViewModel Employee { get; set; }
    }
}

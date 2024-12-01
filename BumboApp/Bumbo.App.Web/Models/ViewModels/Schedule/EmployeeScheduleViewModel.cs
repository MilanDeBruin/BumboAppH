using Bumbo.App.Web.Models.ViewModels.Schedule;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Models.ViewModels
{
    public class EmployeeScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MainFunction { get; set; } = string.Empty;
        public ICollection<ScheduleViewModel> Schedules { get; set; }
        public Dictionary<DayNameOfWeek, ViewModels.Schedule.DaySummary> DailySummaries { get; set; } = new Dictionary<DayNameOfWeek, ViewModels.Schedule.DaySummary>();
    }
}

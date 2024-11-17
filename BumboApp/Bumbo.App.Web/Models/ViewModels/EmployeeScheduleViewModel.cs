using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Models.ViewModels
{
    public class EmployeeScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Schedule> Schedules { get; set; }
    }
}

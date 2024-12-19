namespace Bumbo.App.Web.Models.ViewModels.Schedule
{
    public class ScheduleViewModel
    {
        public int BranchId { get; set; }
        public DateOnly FirstDateOfWeek { get; set; }
        public List<EmployeeScheduleViewModel> EmployeeSchedules { get; set; }
        public List<ForecastData> forecastDatas { get; set; }
    }
}

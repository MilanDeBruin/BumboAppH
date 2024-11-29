namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class WeekPersonalScheduleViewModel
    {
        List<DayPersonalScheduleViewModel>? workDays { get; set; }
        int id_User {get; set; }
        public DateOnly FirstDayOfWeek { get; set; }

    }
}

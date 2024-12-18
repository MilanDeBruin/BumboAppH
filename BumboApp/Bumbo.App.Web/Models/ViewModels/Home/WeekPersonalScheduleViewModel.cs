namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class WeekPersonalScheduleViewModel
    {
       public List<DayPersonalScheduleViewModel> WorkDays { get; set; }

        public DateOnly FirstDayOfWeek { get; set; }

        public Boolean isSick { get; set; }

    }
}

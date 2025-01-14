using Bumbo.Data.Models;

namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class WeekPersonalScheduleViewModel
    {
       public List<DayPersonalScheduleViewModel> WorkDays { get; set; }

        public DateOnly FirstDayOfWeek { get; set; }

        public Boolean isSick { get; set; }

        public Boolean ingeklokt { get; set; }
        
        public List<string>? sickListNames { get; set; }

    }
}

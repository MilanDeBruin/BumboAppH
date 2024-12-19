namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class DayPersonalScheduleViewModel
    {
        public DateOnly Date { get; set; }
        public List<ShiftsViewModel> Shifts { get; set; } 

    }
}

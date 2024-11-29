namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class DayPersonalScheduleViewModel
    {
        public DateOnly date { get; set; }
        public string Departement { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly endTime { get; set;}
        public int Brand_Id { get; set; }   

    }
}

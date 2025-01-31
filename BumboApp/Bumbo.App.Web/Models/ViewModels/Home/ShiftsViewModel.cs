namespace Bumbo.App.Web.Models.ViewModels.Home
{
    public class ShiftsViewModel
    {
        public string Departement { get; set; }
        public int EmployeeId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Branch_Id { get; set; }
        public bool Is_Sick { get; set; }
        public string ShiftStatus { get; set; }
    }
}

namespace Bumbo.App.Web.Models.ViewModels.Dayoverview
{
	public class DayOverviewViewModel
	{
		public int EmployeeId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Decimal PlannedHours { get; set; }
		public Decimal WorkedHours { get; set; }
        public string WorkedHoursString { get; set; }
		public TimeSpan WorkedHoursTimeSpan { get; set; }
        public Decimal Difference => PlannedHours - WorkedHours;
		public int BranchId { get; set; }
	}

}

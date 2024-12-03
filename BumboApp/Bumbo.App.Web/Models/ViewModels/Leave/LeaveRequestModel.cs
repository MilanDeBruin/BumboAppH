namespace Bumbo.App.Web.Models.ViewModels.LeaveRequest
{
    public class LeaveRequestModel
    {

        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public DateOnly start { get; set; }
        public DateOnly end { get; set; }
        public string status { get; set; }
        


    }
}

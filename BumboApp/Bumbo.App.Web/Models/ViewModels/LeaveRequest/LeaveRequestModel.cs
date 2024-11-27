namespace Bumbo.App.Web.Models.ViewModels.LeaveRequest
{
    public class LeaveRequestModel
    {

        public int employeeId { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string status { get; set; }
        public DateTime today { get; set; }


    }
}

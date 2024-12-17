using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Bumbo.App.Web.Models.ViewModels.LeaveRequest
{
    public class LeaveRequestModel
    {

        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public DateOnly start { get; set; }
        public DateOnly end { get; set; }
        public string status { get; set; }

        public List<Bumbo.Data.Models.Leave> myRequests { get; set; }
        public List<string> StatusOptions { get; set; }


    }
}

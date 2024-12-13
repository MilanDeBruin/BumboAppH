using Bumbo.App.Web.Models.ViewModels.Leave;
using Bumbo.App.Web.Models.ViewModels.LeaveRequest;
using Bumbo.Data.Models;
namespace Bumbo.App.Web.Models.ViewModels.Leave
{
    public class AllLeaveRequestsModel
    {

        public List<LeaveRequestModel> myRequests { get; set; }
        public List<List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO>> myRequestsList { get; set; }

        public DateTime startOfWeek { get; set; }
        public DateTime endOfWeek { get; set; }

        public List<DateTime> weekDates { get; set; }
        public List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO> leaves { get; set; }
    }
}

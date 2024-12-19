using Bumbo.Data.Models.LeaveModel;

namespace Bumbo.App.Web.Models.ViewModels.Leave
{
    public class LeaveOverviewModel
    {

        public DateTime startOfWeek {  get; set; }
        public DateTime endOfWeek { get; set; }

        public List<DateTime> weekDates { get; set; }
        public List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO> leaves { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Data.Models.LeaveModel
{
    public class LeaveOverviewDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public object BranchId { get; set; }
    }
}

using Bumbo.Data.Models;
using Bumbo.Data.Models.Cao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bumbo.Domain.Services.MonthOverview
{
    public class MonthOverview : IMonthOverview
    {
        public int DoAllChecks(List<BonusModel> allChecks, List<WorkSchedule> employeeSchedules)
        {
           
            foreach(var workDay in employeeSchedules)
            {
            }

            int hoursToPay = 0;

            foreach (var check in allChecks)
            {

            }
            return hoursToPay;
        }
    }
}

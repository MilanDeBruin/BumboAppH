using Bumbo.Data.Models;
using Bumbo.Data.Models.Cao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Domain.Services.MonthOverview
{
    public interface IMonthOverview
    {
        public int DoAllChecks(List<BonusModel> allChecks,List<WorkSchedule> employeeSchedules);
    }
}

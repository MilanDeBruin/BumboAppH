using Bumbo.Data.Models;
using Bumbo.Data.Models.Cao;
using Bumbo.Domain.Models.MonthOverview;
using BumboApp.Models;
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
            int hoursToPay = 0;
            List<WorkModel> workModels = new List<WorkModel>();

            foreach (var workDay in employeeSchedules)
            {
                WorkModel workModel = new WorkModel
                {
                    EmployeeId = workDay.EmployeeId,
                    Date = workDay.Date,
                    BranchId = workDay.BranchId,
                    StartTime = workDay.StartTime,
                    EndTime = workDay.EndTime,
                    Department = workDay.Department,
                    WorkStatus = workDay.WorkStatus,
                    IsSick = workDay.IsSick,
                    DayOfWeek = workDay.Date.DayOfWeek
                };
                workModels.Add(workModel);
            }

            foreach (var check in allChecks)
            {
                foreach (var workModel in workModels)
                {
                    if (check.DayOfWeek == DayOfWeek.Sunday && workModel.DayOfWeek == DayOfWeek.Sunday) 
                    {
                        int verschilInUren = (workModel.EndTime - workModel.StartTime).Hours;
                        verschilInUren = verschilInUren * (int)(check.BonusPercentage + 1);
                    }
                }
            }
            return hoursToPay;
        }
    }
}

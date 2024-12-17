using Bumbo.Data.Models;
using Bumbo.Data.Models.LeaveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Data.Interfaces
{
    public interface ILeaveRepository
    {
        public void SetLeaveRequest(Leave request);

        public List<string> GetLeaveStatuses();

        public void updateLeaveStatus(Leave request);

        public Leave getLeaveRequest(int id, DateOnly StartDate);

        public List<Leave> getAllRequestsOfEmployee(int id);

        public List<Leave> getAllPendingRequests();
        public List<Leave> getAllRequests();


        public List<LeaveOverviewDTO> getAllLeaves(DateOnly startDate, DateOnly endDate);

        public Boolean getOverlap(DateOnly StartDate, DateOnly endDate, int id);

        public Boolean checkStartDateForDuble(int id , DateOnly startDate);

        public List<int> getEmployeesInLeave();

        public void UpdateLeaveRequest(int employeeId, DateOnly startDate, string status);



    }
}

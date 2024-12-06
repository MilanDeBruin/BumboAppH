using Bumbo.Data.Models;
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

        public List<Leave> getAllRequests();

        public Boolean getOverlap(DateOnly StartDate, DateOnly endDate, int id);
    }
}

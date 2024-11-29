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

        public List<Leave> getAllRequestsOfEmployee(int id);
    }
}

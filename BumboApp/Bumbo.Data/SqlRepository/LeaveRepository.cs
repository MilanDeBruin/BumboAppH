using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Bumbo.Data.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Data.SqlRepository
{
    public class LeaveRepository : ILeaveRepository
    {
        readonly BumboDbContext ctx;

        public LeaveRepository(BumboDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void SetLeaveRequest(Leave request)
        {
            ctx.Leaves.Add(request);
            ctx.SaveChanges();
        }

        public List<string> GetLeaveStatuses() => ctx.LeaveStatuses.Select(n => n.LeaveStatus1).ToList();

        public List<Leave> getAllRequestsOfEmployee(int id) => ctx.Leaves.Where(n => n.EmployeeId == id).ToList();

        public List<Leave>getAllRequests() => ctx.Leaves.Where(n => n.LeaveStatus == "Requested").OrderBy(n => n.EmployeeId).ToList();

        
    }
}

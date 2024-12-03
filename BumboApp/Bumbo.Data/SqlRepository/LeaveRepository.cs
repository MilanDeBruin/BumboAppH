using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

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

        public void updateLeaveStatus(Leave request)
        {
            ctx.Update(request);
            ctx.SaveChanges();
        }

        public List<Leave> getAllRequestsOfEmployee(int id) => ctx.Leaves.Where(n => n.EmployeeId == id).OrderBy(n => n.StartDate).ToList();

        public List<Leave> getAllRequests() => ctx.Leaves.Where(n => n.LeaveStatus == "Requested").OrderBy(n => n.EmployeeId).ToList();

        public Leave getLeaveRequest(int id, DateOnly StartDate)
        {
            return ctx.Leaves
              .Where(n => n.EmployeeId == id && n.StartDate == StartDate)
              .FirstOrDefault();
        }
    }
}

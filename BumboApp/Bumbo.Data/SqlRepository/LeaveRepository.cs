using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Data.Models.LeaveModel;


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

        public List<LeaveOverviewDTO> getAllLeaves(DateOnly startDate, DateOnly endDate)
        {
            var result = (from leave in ctx.Leaves
                          join employee in ctx.Employees
                          on leave.EmployeeId equals employee.EmployeeId
                          where
                              leave.LeaveStatus == "Accepted" &&
                              (leave.StartDate >= startDate && leave.StartDate <= endDate ||
                               leave.EndDate >= startDate && leave.EndDate <= endDate)
                          orderby leave.EmployeeId
                          select new LeaveOverviewDTO
                          {
                              FirstName = employee.FirstName,
                              LastName = employee.LastName,
                              StartDate = leave.StartDate,
                              EndDate = leave.EndDate,
                              BranchId = employee.BranchId
                          }).ToList();
            return result;
        }


        public Leave getLeaveRequest(int id, DateOnly StartDate)
        {
            return ctx.Leaves
              .Where(n => n.EmployeeId == id && n.StartDate == StartDate)
              .FirstOrDefault();
        }
        public Boolean getOverlap(DateOnly StartDate, DateOnly endDate , int id)
        {
            Boolean result = false;

            result = (0 == ctx.Leaves.Where(n => n.StartDate >= StartDate && n.EndDate <= endDate && n.EmployeeId == id).Count());
            return result;
        }

       
    }
}

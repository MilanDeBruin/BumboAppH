using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
namespace Bumbo.Domain.Services.Leaves
{
    public class LeaveChecker : ILeaveChecker
    {

        private readonly ILeaveRepository repo;

        public LeaveChecker(ILeaveRepository repo)
        {
            this.repo = repo;
        }

        public int CheckRequestStartDate(Leave request)
        {
            if (request.StartDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public int CheckOverlap(Leave leaveRequest)
        {
            if (repo.getOverlap(leaveRequest.StartDate, leaveRequest.EndDate, leaveRequest.EmployeeId))
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }

        

        public int checkStartDateForDuble(Leave request)
        {
            if (repo.checkStartDateForDuble(request.EmployeeId, request.StartDate))
            {
                return 3;
            }
            else
            {
                return -1;
            }
        }

        public int CheckstartDateHigherThanEndDate(Leave request)
        {
            if ((request.StartDate >= request.EndDate))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        public int doAllChecks(Leave request)
        {
            int checkResult = 0;

            checkResult = CheckRequestStartDate(request);
            if (checkResult > 0)
            {
                return checkResult;
            }

            checkResult = CheckstartDateHigherThanEndDate(request);
            if (checkResult > 0)
            {
                return checkResult;
            }
            checkResult = checkStartDateForDuble(request);
            if (checkResult > 0)
            {
                return checkResult;
            }
            checkResult = CheckOverlap(request);
            return checkResult;
        }
    }
}

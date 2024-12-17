using Bumbo.Data.Models;
namespace Bumbo.Domain.Services.Leaves
{
    public class LeaveChecker : ILeaveChecker
    {
        public Boolean startDateHigherThanEndDate(Leave request)
        {
            return (request.StartDate <= request.EndDate);
        }

        public Boolean checkForOverlap(List<Leave> allRequests, Leave request)
        {
            if (request.StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return false;
            }
            foreach (Leave oldrequest in allRequests)
            {
                // checks if the new requests start and end date are before an old requests startdate
                if (request.StartDate < oldrequest.StartDate && request.EndDate < oldrequest.StartDate ) 
                {
                    return true;
                }
                // checks if the new requests start date is in between an old request start and enddate
                if(request.StartDate > oldrequest.StartDate && request.StartDate < oldrequest.EndDate)
                {
                    return false;
                }
                // checks if the new requests end date is in between an old request start and enddate
                if (request.EndDate > oldrequest.StartDate && request.EndDate > oldrequest.EndDate )
                {
                    return false ;
                }
            }

            return true;
        }
    }
}

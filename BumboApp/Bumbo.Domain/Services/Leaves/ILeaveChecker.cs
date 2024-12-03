using Bumbo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Domain.Services.Leaves
{
    public interface ILeaveChecker
    {
        public Boolean startDateHigherThanEndDate(Leave request);
        public Boolean checkForOverlap(List<Leave> allRequests, Leave request);
    }
}

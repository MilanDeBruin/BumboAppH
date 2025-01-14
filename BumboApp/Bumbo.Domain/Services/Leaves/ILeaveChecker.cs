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
        public int CheckRequestStartDate(Leave request);
        public int CheckstartDateHigherThanEndDate(Leave request);
        public int checkStartDateForDuble(Leave request);
        public int CheckOverlap(Leave leaveRequest);
        public int doAllChecks(Leave request);
    }
}

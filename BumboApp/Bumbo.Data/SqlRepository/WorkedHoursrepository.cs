using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Data.SqlRepository
{
    public class WorkedHoursrepository : IWorkedHoursRepository
    {
        readonly BumboDbContext _context;

        public WorkedHoursrepository(BumboDbContext context)
        {
            _context = context;
        }

        public List<WorkShift> getWorksifts(int employeeId)
        {
            return _context.WorkShifts.Where(ws => ws.EmployeeId == employeeId).ToList();
        }


    }
}

using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;

namespace Bumbo.Data.SqlRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        readonly BumboDbContext ctx;

        public EmployeeRepository(BumboDbContext ctx)
        {
            this.ctx = ctx;
        }


        public string FindNameFromId(int id)
        {
            var employee = ctx.Employees
                .Where(e => e.EmployeeId == id)
                .Select(e => new { e.FirstName, e.LastName })
                .FirstOrDefault();

            return employee != null
                ? $"{employee.FirstName} {employee.LastName}"
                : "Employee not found";
        }
    }
}

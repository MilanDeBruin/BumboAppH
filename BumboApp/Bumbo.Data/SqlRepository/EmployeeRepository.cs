using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;


namespace Bumbo.Data.SqlRepository
{
    public class EmployeeRepository(BumboDbContext ctx) : IEmployeeRepository
    {

        readonly BumboDbContext ctx = ctx;

        public Employee? GetEmployee(int id)
        {
            return ctx.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public IEnumerable<Employee> GetEmployees(int branchId)
        {
            return [.. ctx.Employees.Where(e => e.BranchId == branchId)];
        }

        public void SaveEmployee(Employee employee)
        {
            ctx.Employees.Add(employee);
            ctx.SaveChanges();
        }

        public bool UpdateEmployee(Employee employee)
        {
            var existingEmployee = ctx.Employees.Find(employee.EmployeeId);
            if (existingEmployee == null) return false;

            existingEmployee.BranchId = employee.BranchId;
            existingEmployee.Position = employee.Position;
            existingEmployee.HiringDate = employee.HiringDate;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.Infix = employee.Infix;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.HouseNumber = employee.HouseNumber;
            existingEmployee.Addition = employee.Addition;
            existingEmployee.ZipCode = employee.ZipCode;
            existingEmployee.EmailAdres = employee.EmailAdres;
            existingEmployee.Password = employee.Password;
            existingEmployee.LaborContract = employee.LaborContract;

            ctx.SaveChanges();
            return true;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var employee = ctx.Employees.Find(employeeId);
            if (employee == null) return false;

            ctx.Employees.Remove(employee);
            ctx.SaveChanges();
            return true;
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

        public IEnumerable<Branch> GetBranches()
        {
            return [.. ctx.Branches];
        }

        public IEnumerable<Position> GetPositions()
        {
            return [.. ctx.Positions];
        }

        public IEnumerable<LaborContract> GetLaborContracts()
        {
            return ctx.LaborContracts.ToList();
        }
    }
}
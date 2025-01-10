using Bumbo.Data.Models;
using Bumbo.Domain.Enums;

namespace Bumbo.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        public Employee? GetEmployee(int id);
        public Employee? GetEmployee(string userId);
        public IEnumerable<Employee> GetEmployees(int branchId);
        public void SaveEmployee(Employee employee, string email, string password, RoleEnum role); 
        public bool UpdateEmployee(Employee employee);
        public bool DeleteEmployee(int employeeId);
        public string FindNameFromId(int id);
        public string getRoles(string userId);
        public IEnumerable<Branch> GetBranches();
        public IEnumerable<Position> GetPositions();
        public IEnumerable<LaborContract> GetLaborContracts();
    }
}


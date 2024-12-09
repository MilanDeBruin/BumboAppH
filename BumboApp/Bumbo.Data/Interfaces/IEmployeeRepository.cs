using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        
        public Employee? GetEmployee(int id);
        public IEnumerable<Employee> GetEmployees(int branchId);
        public void SaveEmployee(Employee employee);
        public bool UpdateEmployee(Employee employee);
        public bool DeleteEmployee(int employeeId);
        public string FindNameFromId(int id);
        public IEnumerable<Branch> GetBranches();
        public IEnumerable<Position> GetPositions();
        public IEnumerable<LaborContract> GetLaborContracts();
    }
}


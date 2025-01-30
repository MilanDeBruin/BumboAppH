using Bumbo.Data.Models;
using Bumbo.Domain.Enums;

namespace Bumbo.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        public Employee? GetEmployeeByEmployeeId(int id);
        public Employee? GetEmployeeByUserId(string userId);
        public IQueryable<Employee> GetAllEmployeesByBranchId(int branchId);
        public void AddEmployee(Employee employee, string email, string password, RoleEnum role); 
        public bool UpdateEmployee(Employee employee, string emailAdres,  string? password);
        public bool DeleteEmployee(int employeeId);
        public string FindNameFromId(int id);
        public string? FindEmailFromUserId(string userId);
        public string GetRoles(string userId);
    }
}


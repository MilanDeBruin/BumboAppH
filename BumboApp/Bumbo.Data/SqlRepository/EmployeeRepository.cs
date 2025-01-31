using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.SqlRepository
{
    public class EmployeeRepository(BumboDbContext context, UserManager<IdentityUser> userManager) : IEmployeeRepository
    {
        readonly BumboDbContext _context = context;
        readonly UserManager<IdentityUser> _userManager = userManager;

        public Employee? GetEmployeeByEmployeeId(int id)
        {
            var employeeToRead = _context.Employees
                .FirstOrDefault(e => e.EmployeeId == id);
            
            return employeeToRead;
        }

        public Employee? GetEmployeeByUserId(string userId)
        {
            var employeeToRead = _context.Employees
                .FirstOrDefault(e => e.UserId == userId);
            
            return employeeToRead;
        }
        
        public IQueryable<Employee> GetAllEmployeesByBranchId(int branchId)
        {
            var employeesToRead = _context.Employees
                .Where(e => e.BranchId == branchId);
            
            return employeesToRead;
        }

        public void AddEmployee(Employee employee, string email, string password, RoleEnum role)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
            };
            
            var result = _userManager.CreateAsync(user, password).Result;
            var newUser = _userManager.FindByEmailAsync(email).Result;

            if (newUser != null)
            {
                _userManager.AddToRoleAsync(newUser, role.ToString().ToLower()).Wait();
                employee.UserId = newUser.Id;
            }
            
            if (!result.Succeeded) return;
            
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public bool UpdateEmployee(Employee employee, string emailAdres, string? password)
        {
            var existingEmployee = _context.Employees.Find(employee.EmployeeId);
            if (existingEmployee == null) return false;

            existingEmployee.BranchId = employee.BranchId;
            existingEmployee.HiringDate = employee.HiringDate;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.Infix = employee.Infix;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.HouseNumber = employee.HouseNumber;
            existingEmployee.Addition = employee.Addition;
            existingEmployee.ZipCode = employee.ZipCode;
            if (!string.IsNullOrEmpty(emailAdres)) ChangeUsername(existingEmployee.UserId, emailAdres);
            if (!string.IsNullOrEmpty(password)) ChangePassword(existingEmployee.UserId, password);
            existingEmployee.LaborContract = employee.LaborContract;

            _context.SaveChanges();
            return true;
        }
        
        private void ChangePassword(string userId, string password)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null) return;

            _userManager.RemovePasswordAsync(user).Wait();
            _userManager.AddPasswordAsync(user, password).Wait();
        }
        
        private void ChangeUsername(string userId, string email)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null) return;

            user.Email = email;
            user.UserName = email;
            _userManager.UpdateAsync(user).Wait();
        }

        public bool DeleteEmployee(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return false;
            var user = _userManager.FindByIdAsync(employee.UserId).Result;
            if (user != null) _userManager.DeleteAsync(user).Wait();
            return true;
        }

        public string FindNameFromId(int id)
        {
            var employee = _context.Employees
                .Where(e => e.EmployeeId == id)
                .Select(e => new { e.FirstName, e.LastName })
                .FirstOrDefault();

            return employee != null
                ? $"{employee.FirstName} {employee.LastName}"
                : "Employee not found";
        }
        
        public string? FindEmailFromUserId(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            return user?.Email;
        }
        
        public string GetRoles(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null) return "User not found";

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(); // TODO: Currently only returns the first role, not all roles (since a user can have multiple roles)
            return role ?? "Role not found";
        }
    }
}

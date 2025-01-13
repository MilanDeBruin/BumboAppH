using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.SqlRepository
{
    public class EmployeeRepository(BumboDbContext ctx, UserManager<IdentityUser> userManager) : IEmployeeRepository
    {
        readonly BumboDbContext ctx = ctx;
        readonly UserManager<IdentityUser> userManager = userManager;

        public Employee? GetEmployee(int id)
        {
            return ctx.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public Employee? GetEmployee(string userId)
        {
            return ctx.Employees.FirstOrDefault(e => e.UserId == userId);
        }
        
        public IEnumerable<Employee> GetEmployees(int branchId)
        {
            return [.. ctx.Employees.Where(e => e.BranchId == branchId)];
        }

        public void SaveEmployee(Employee employee, string email, string password, RoleEnum role)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
            };
            
            var result = userManager.CreateAsync(user, password).Result;
            var newUser = userManager.FindByEmailAsync(email).Result;

            if (newUser != null)
            {
                userManager.AddToRoleAsync(newUser, role.ToString().ToLower()).Wait();
                employee.UserId = newUser.Id;
            }
            
            if (!result.Succeeded) return;
            
            ctx.Employees.Add(employee);
            ctx.SaveChanges();
        }

        public bool UpdateEmployee(Employee employee, string emailAdres, string password)
        {
            var existingEmployee = ctx.Employees.Find(employee.EmployeeId);
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

            ctx.SaveChanges();
            return true;
        }
        
        private void ChangePassword(string userId, string password)
        {
            var user = userManager.FindByIdAsync(userId).Result;
            if (user == null) return;

            userManager.RemovePasswordAsync(user).Wait();
            userManager.AddPasswordAsync(user, password).Wait();
        }
        
        private void ChangeUsername(string userId, string email)
        {
            var user = userManager.FindByIdAsync(userId).Result;
            if (user == null) return;

            user.Email = email;
            user.UserName = email;
            userManager.UpdateAsync(user).Wait();
        }
        
        public bool DeleteEmployee(int employeeId)
        {
            var employee = ctx.Employees.Find(employeeId);
            if (employee == null) return false;
            var user = userManager.FindByIdAsync(employee.UserId).Result;
            if (user != null) userManager.DeleteAsync(user).Wait();

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
        
        public string GetRoles(string userId)
        {
            var user = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null) return "User not found";

            var role = userManager.GetRolesAsync(user).Result.FirstOrDefault(); // TODO: Currently only returns the first role, not all roles (since a user can have multiple roles)
            return role ?? "Role not found";
        }

        public IEnumerable<Branch> GetBranches()
        {
            return [.. ctx.Branches];
        }

        public IEnumerable<LaborContract> GetLaborContracts()
        {
            return ctx.LaborContracts.ToList();
        }
    }
}

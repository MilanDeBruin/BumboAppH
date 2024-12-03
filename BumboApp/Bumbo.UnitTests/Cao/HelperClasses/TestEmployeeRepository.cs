using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestEmployeeRepository : IEmployeeRepository
{
    public Employee GetEmployee(int id)
    {
        if (id == 3)
        {
            return new Employee()
            {
                EmployeeId = id,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-15))
            };
        }
        
        return new Employee()
        {
            EmployeeId = id,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };
    }
}
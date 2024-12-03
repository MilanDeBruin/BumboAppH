using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestEmployeeRepository : IEmployeeRepository
{
    public Employee GetEmployee(int id)
    {
        // employee aged 15
        // used in max endtime test and max daily workhour test
        if (id == 3)
        {
            return new Employee()
            {
                EmployeeId = id,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-15))
            };
        }
        
        //employee aged 17
        // used in max endtime test and max daily workhour test
        if (id == 4)
        {
            return new Employee()
            {
                EmployeeId = id,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-15))
            };
        }
        
        // general adult employee
        return new Employee()
        {
            EmployeeId = id,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };
    }
}
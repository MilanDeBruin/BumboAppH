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
        
        // employee aged 15
        // used in maximum amount of weekly days
        if (id == 5)
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

    public IEnumerable<Employee> GetEmployees(int branchId)
    {
        throw new NotImplementedException();
    }

    public void SaveEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool UpdateEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool DeleteEmployee(int employeeId)
    {
        throw new NotImplementedException();
    }

    public string FindNameFromId(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Branch> GetBranches()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Position> GetPositions()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LaborContract> GetLaborContracts()
    {
        throw new NotImplementedException();
    }
}
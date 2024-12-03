using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IEmployeeRepository
{
    public Employee GetEmployee(int id);
}
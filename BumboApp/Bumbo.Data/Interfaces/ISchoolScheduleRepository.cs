using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface ISchoolScheduleRepository
{
    public List<SchoolSchedule> GetWeekSchoolScheduleForEmployee(int employeeId, DateOnly date);

}
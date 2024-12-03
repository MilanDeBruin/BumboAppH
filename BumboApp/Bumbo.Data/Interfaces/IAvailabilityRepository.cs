using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IAvailabilityRepository
{
    public SchoolSchedule GetDailySchoolSchedule(int employeeId, DayOfWeek dayOfWeek);
}
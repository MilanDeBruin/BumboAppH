using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestAvailabilityRepository : IAvailabilityRepository
{
    public SchoolSchedule GetDailySchoolSchedule(int employeeId, DayOfWeek dayOfWeek)
    {
        return new SchoolSchedule();
    }
}
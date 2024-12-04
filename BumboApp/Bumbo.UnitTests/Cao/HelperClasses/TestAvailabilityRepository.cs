using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestAvailabilityRepository : IAvailabilityRepository
{
    public SchoolSchedule GetDailySchoolSchedule(int employeeId, DayOfWeek dayOfWeek)
    {
        return new SchoolSchedule();
    }

    public TimeOnly GetStoreOpeningHour(int branchId, DateOnly dayofWeek)
    {
        throw new NotImplementedException();
    }

    public TimeOnly GetStoreClosingHour(int branchId, DateOnly dayofWeek)
    {
        throw new NotImplementedException();
    }

    public List<Availability> GetWeekAvailability(int branchId, DateOnly firstDayOfWeek)
    {
        throw new NotImplementedException();
    }
}
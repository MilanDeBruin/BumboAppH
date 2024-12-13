using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IAvailabilityRepository
{

    public SchoolSchedule? GetDailySchoolSchedule(int employeeId, DayOfWeek dayOfWeek);

    public TimeOnly GetStoreOpeningHour(int branchId, DateOnly dayofWeek);
    public TimeOnly GetStoreClosingHour(int branchId, DateOnly dayofWeek);

    public List<Availability> GetWeekAvailability(int branchId, DateOnly firstDayOfWeek);
    public List<Availability> GetWeekAvailabilityForEmployee(int employeeId, DateOnly date);
}
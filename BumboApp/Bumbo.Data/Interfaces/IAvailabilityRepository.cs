using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IAvailabilityRepository
{
    public TimeOnly GetStoreOpeningHour(int branchId, DateOnly dayofWeek);
    public TimeOnly GetStoreClosingHour(int branchId, DateOnly dayofWeek);

    public List<Availability> GetWeekAvailability(int branchId, DateOnly firstDayOfWeek);
}
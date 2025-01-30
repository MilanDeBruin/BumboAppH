using Bumbo.Data.Context;
using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IScheduleRepository
{
    public List<WorkSchedule> GetWeeklyWorkSchedules(DateOnly firstDayOfWeek, int employeeId);

    public List<WorkSchedule> GetAmountOfWeeksWorkSchedule(DateOnly firstDateOfWeeks, int employeeId, int amountOfWeeks);
    public WorkSchedule GetSchedule(int employee, int branch, DateOnly date, TimeOnly startTime);
}
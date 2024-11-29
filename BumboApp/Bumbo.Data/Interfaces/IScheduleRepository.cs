using Bumbo.Data.Context;
using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IScheduleRepository
{
    public List<WorkSchedule> GetWeeklyWorkSchedules(DateOnly firstDayOfWeek, int employeeId);
}
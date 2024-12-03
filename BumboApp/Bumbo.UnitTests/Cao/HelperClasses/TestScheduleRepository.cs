using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestScheduleRepository : IScheduleRepository
{
    public List<WorkSchedule> GetWeeklyWorkSchedules(DateOnly firstDayOfWeek, int employeeId)
    {
        // for breaktime tests
        if (employeeId == 2)
        {
            List<WorkSchedule> schedules = new List<WorkSchedule>();
            // monday
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 2),
                StartTime = new TimeOnly(12, 0, 0),
                EndTime = new TimeOnly(14, 0, 0)
            });
            
            // tuesday
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 3),
                StartTime = new TimeOnly(12, 0, 0),
                EndTime = new TimeOnly(14, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 3),
                StartTime = new TimeOnly(14, 30, 0),
                EndTime = new TimeOnly(16, 0, 0)
            });
            
            // wednesday
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 4),
                StartTime = new TimeOnly(12, 0, 0),
                EndTime = new TimeOnly(14, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 4),
                StartTime = new TimeOnly(14, 30, 0),
                EndTime = new TimeOnly(16, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 4),
                StartTime = new TimeOnly(16, 30, 0),
                EndTime = new TimeOnly(18, 0, 0)
            });
            return schedules;
        }

        return new List<WorkSchedule>();
    }

    public List<WorkSchedule> GetAmountOfWeeksWorkSchedule(DateOnly firstDateOfWeeks, int employeeId, int amountOfWeeks)
    {
        throw new NotImplementedException();
    }
    
}
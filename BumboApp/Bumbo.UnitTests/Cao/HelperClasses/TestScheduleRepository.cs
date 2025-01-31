using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.UnitTests.Cao.HelperClasses;

public class TestScheduleRepository : IScheduleRepository
{
    public List<WorkSchedule> GetWeeklyWorkSchedules(DateOnly firstDayOfWeek, int employeeId)
    {
        // for breaktime tests and max daily workhours test
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
            
            //thursday
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 5),
                StartTime = new TimeOnly(8, 0, 0),
                EndTime = new TimeOnly(12, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 5),
                StartTime = new TimeOnly(13, 0, 0),
                EndTime = new TimeOnly(17, 0, 0)
            });
            return schedules;
        }

        // used in max endtime test and max daily workhour test
        // monday empty
        // tuesday 1 shift
        
        if (employeeId == 3)
        {
            List<WorkSchedule> schedules = new List<WorkSchedule>();
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 3),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(14, 0, 0)
            });

            return schedules;

        }

        // used in max amount of days
        // has one day less than maximum amount of days
        if (employeeId == 5 && firstDayOfWeek == new DateOnly(2024, 12, 02))
        {
            List<WorkSchedule> schedules = new List<WorkSchedule>();
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 2),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 3),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 4),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 5),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            return schedules;
        }
        
        // used in max amount of days
        // has exactly max amount of days
        if (employeeId == 5 && firstDayOfWeek == new DateOnly(2024, 12, 09))
        {
            List<WorkSchedule> schedules = new List<WorkSchedule>();
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 9),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 10),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 11),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 12),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            schedules.Add(new WorkSchedule()
            {
                Date = new DateOnly(2024, 12, 13),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(11, 0, 0)
            });
            return schedules;
        }
        
        

        return new List<WorkSchedule>();
    }

    public List<WorkSchedule> GetAmountOfWeeksWorkSchedule(DateOnly firstDateOfWeeks, int employeeId, int amountOfWeeks)
    {
        return new List<WorkSchedule>();
    }

    public WorkSchedule GetSchedule(int employee, int branch, DateOnly date, TimeOnly startTime)
    {
        throw new NotImplementedException();
    }
}
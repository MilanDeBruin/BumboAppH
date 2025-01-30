using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class ScheduleRepository : IScheduleRepository
{

    private readonly BumboDbContext _dbContext;

    public ScheduleRepository(BumboDbContext context)
    {
        _dbContext = context;
    }

    public List<WorkSchedule> GetWeeklyWorkSchedules(DateOnly firstDayOfWeek, int employeeId)
    {
            return _dbContext.WorkSchedules.Where(s =>
                s.EmployeeId == employeeId && s.Date >= firstDayOfWeek && s.Date < firstDayOfWeek.AddDays(7)).ToList();
    }

    public List<WorkSchedule> GetAmountOfWeeksWorkSchedule(DateOnly firstDateOfWeeks, int employeeId, int amountOfWeeks)
    {
        return _dbContext.WorkSchedules.Where(s =>
            s.EmployeeId == employeeId && s.Date >= firstDateOfWeeks && s.Date < firstDateOfWeeks.AddDays(7*4)).ToList();
    }

    public WorkSchedule GetSchedule(int employee, int branch, DateOnly date, TimeOnly startTime)
    {
        return _dbContext.WorkSchedules.First(ws => ws.EmployeeId == employee && ws.BranchId == branch && ws.Date == date);
    }
    
}
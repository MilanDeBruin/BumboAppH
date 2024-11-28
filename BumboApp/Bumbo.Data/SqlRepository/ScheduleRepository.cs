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
    
}
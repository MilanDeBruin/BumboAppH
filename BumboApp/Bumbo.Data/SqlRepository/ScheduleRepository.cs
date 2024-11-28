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
        throw new NotImplementedException();
    }
    
}
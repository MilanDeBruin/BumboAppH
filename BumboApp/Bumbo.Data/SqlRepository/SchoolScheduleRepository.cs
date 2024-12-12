using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class SchoolScheduleRepository : ISchoolScheduleRepository
{
    private readonly BumboDbContext _dbContext;

    public SchoolScheduleRepository(BumboDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public List<SchoolSchedule> GetWeekSchoolScheduleForEmployee(int employeeId, DateOnly firstDayOfWeek)
    {
        List<string> daysOfWeek = new List<string>();
        for (int i = 0; i < 7; i++)
        {
            daysOfWeek.Add(firstDayOfWeek.AddDays(i).ToString("dddd", new System.Globalization.CultureInfo("nl-NL")));
        }

        List<SchoolSchedule> weekSchoolSchedule = _dbContext.SchoolSchedules
            .Where(a => a.EmployeeId == employeeId && daysOfWeek.Contains(a.Weekday))
            .ToList();

        return weekSchoolSchedule;
    }
}

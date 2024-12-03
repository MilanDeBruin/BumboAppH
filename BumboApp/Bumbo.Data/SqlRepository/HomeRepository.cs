using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.Data.SqlClient;


namespace Bumbo.Data.SqlRepository;

public class HomeRepository : IHomeRepository
{
    private readonly BumboDbContext _db;

    public HomeRepository(BumboDbContext db)
    {
        _db = db;
    }

    public List<WorkSchedule> GetScheduleData(int employeeId, DateOnly firstDayOfWeek)
    {

        List<WorkSchedule> list = _db.WorkSchedules.Where(ws => ws.EmployeeId == employeeId && ws.Date >= firstDayOfWeek && ws.Date <= firstDayOfWeek.AddDays(7)).ToList();

        return list;
    }

}
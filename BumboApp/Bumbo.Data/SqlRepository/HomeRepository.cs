using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;


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

    public void SetSick(int employeeId, DateOnly firstDayOfWeek)
    {
        var workSchedules = _db.WorkSchedules
         .Where(ws => ws.EmployeeId == employeeId && ws.Date == firstDayOfWeek).ToList();
        
        if (workSchedules != null)
        {
            foreach (var workSchedule in workSchedules)
            {
               
                workSchedule.IsSick = true;
            }
        }
        _db.SaveChanges();
    }

    public Boolean GetSick(int employeeId)
    {

        var sick = _db.WorkSchedules.Where(ws => ws.EmployeeId == employeeId && ws.Date == DateOnly.FromDateTime(DateTime.Now)).ToList();

        bool isSick = sick.Any(ws => ws.IsSick);
        return isSick;
    }

    public List<string> getSickList()
    {
        var sicklist = (from ws in _db.WorkSchedules
         join em in _db.Employees on ws.EmployeeId equals em.EmployeeId
         where ws.IsSick == true && ws.Date == DateOnly.FromDateTime(DateTime.Now)
         select em.FirstName).ToList();
        return sicklist;
    }

    public void Inklokken(int employeeId)
    {
       
    }

    public void Uitklokken(int employeeId)
    {

    }

}
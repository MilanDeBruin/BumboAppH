using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.Identity.Client;

namespace Bumbo.Data.SqlRepository;

public class HomeRepository : IHomeRepository
{
    private readonly BumboDbContext _db;

    public HomeRepository(BumboDbContext db)
    {
        _db = db;
    }

    public List<WorkSchedule> GetScheduleData()
    {
        int id_Employee = 1;
        List<WorkSchedule> list = _db.WorkSchedules.Where(ws => ws.EmployeeId == id_Employee).ToList();
        return list;
    }
    

    
}
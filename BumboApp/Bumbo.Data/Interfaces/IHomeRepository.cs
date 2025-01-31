using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;

namespace Bumbo.Data.Interfaces;

public interface IHomeRepository 
{
    public List<WorkSchedule> GetScheduleData(int employeeId, DateOnly firstDayOfWeek);
    public void SetSick(int employeeId, DateOnly firstDayOfWeek);
    public Boolean GetSick(int employeeIf);
    public List<string> getSickList();
    public void Inklokken(int employeeId, DateTime currentTime);
    public void Uitklokken(int employeeId, DateTime currentTime);
    public Boolean GetIngeklokt(int employeeId);
    public Boolean CheckShift(int employeeId);
    public Boolean CheckStartTime(int employeeId, DateTime currentTime);
    public void DeleteShift(int employeeId, DateTime currentTime);

}
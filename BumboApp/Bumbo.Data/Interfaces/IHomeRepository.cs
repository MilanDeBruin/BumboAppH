using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;

namespace Bumbo.Data.Interfaces;

public interface IHomeRepository 
{
    public List<WorkSchedule> GetScheduleData(int employeeId, DateOnly firstDayOfWeek);
    public void SetSick(int employeeId, DateOnly firstDayOfWeek);
    public Boolean GetSick(int employeeIf);
    public List<string> getSickList();
    public void Inklokken(int employeeId);
    public void Uitklokken(int employeeId);
    public Boolean GetIngeklokt(int employeeId);
    public Boolean CheckShift(int employeeId);
    public DateTime getStartTime(int employeeId);
}
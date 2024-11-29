using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;

namespace Bumbo.Data.Interfaces;

public interface IHomeRepository 
{
    public List<WorkSchedule> GetScheduleData();
}
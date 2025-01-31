using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces
{
    public interface IWorkedHoursRepository 
    {
        public List<WorkShift> getWorksifts(int employeeId);
    }
}
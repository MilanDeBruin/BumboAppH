using Bumbo.Data.Models.Cao;

namespace Bumbo.Data.Interfaces;

public interface ICaoRepository
{
    public List<BonusModel> GetAllBonus();
    public SickSalaryRate GetSickSalaryRate();
    public List<WorkHourRestrictionModel> GetWorkHourRestrictions();
    public BreakTimeModel GetBreakTime();
}
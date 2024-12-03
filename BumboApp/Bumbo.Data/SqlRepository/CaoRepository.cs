using Bumbo.Data.Interfaces;
using Bumbo.Data.Models.Cao;

namespace Bumbo.Data.SqlRepository;

public class CaoRepository : ICaoRepository
{
    public SickSalaryRate GetSickSalaryRate()
    {
        return new SickSalaryRate()
        {
            RatePercentage = 70
        };
    }
    
    public BreakTimeModel GetBreakTime()
    {
        return new BreakTimeModel()
        {
            MaxConsecutiveWorkTime = new TimeSpan(4, 30, 0),
            MinBreakTime = new TimeSpan(0, 30, 0)
        };
    }
    
    public List<BonusModel> GetAllBonus()
    {
        List<BonusModel> list = new List<BonusModel>();
        
        list.Add(new BonusModel()
        {
            BeginTime = new TimeOnly(0, 0),
            EndTIme = new TimeOnly(6, 0),
            BonusPercentage = 50,
        });
        list.Add(new BonusModel()
        {
            BeginTime = new TimeOnly(20, 0),
            EndTIme = new TimeOnly(21, 0),
            BonusPercentage = (double) 100 / 3
        });
        list.Add(new BonusModel()
        {
            BeginTime = new TimeOnly(21, 0),
            EndTIme = new TimeOnly(0, 0),
            BonusPercentage = 50
        });
        list.Add(new BonusModel()
        {
            BeginTime = new TimeOnly(18, 0),
            EndTIme = new TimeOnly(0,0),
            DayOfWeek = DayOfWeek.Saturday,
            BonusPercentage = 50
        });
        list.Add(new BonusModel()
        {
            DayOfWeek = DayOfWeek.Sunday,
            BonusPercentage = 100
        });

        return list;
    }
    

    public List<WorkHourRestrictionModel> GetWorkHourRestrictions()
    {
        List<WorkHourRestrictionModel> list = new List<WorkHourRestrictionModel>();
        
        // minors <16
        list.Add(new WorkHourRestrictionModel()
        {
            MaxAge = 15,
            MaxAmountOfDaysPerWeek = 5,
            MaxAmountOfTimePerDay = new TimeSpan(8, 0, 0),
            MaxAmountOfTimePerDayIncludesSchool = true,
            MaxAmountOfTimePerWeek = new TimeSpan(12, 0, 0),
            MaxEndTime = new TimeOnly(19, 0)
        });
        
        //minors 16,17
        list.Add(new WorkHourRestrictionModel()
        {
            MaxAge = 17,
            MaxAmountOfTimePerDay = new TimeSpan(9, 0, 0),
            MaxAmountOfTimePerDayIncludesSchool = true,
            MaxAverageAmountOfTimePerWeeksAmount = 4,
            MaxAverageAmountOfTimePerAmountOfWeeks = new TimeSpan(40, 0, 0)
        });
        
        //general
        list.Add(new WorkHourRestrictionModel()
        {
            MaxAmountOfTimePerDay = new TimeSpan(12, 0, 0),
            MaxAmountOfTimePerWeek = new TimeSpan(60, 0, 0)
        });

        return list;
    }
}
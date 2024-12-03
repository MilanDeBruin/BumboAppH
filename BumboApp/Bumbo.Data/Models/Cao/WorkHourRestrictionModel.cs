namespace Bumbo.Data.Models.Cao;

public class WorkHourRestrictionModel
{
    public int? MaxAge { get; set; }
    public TimeSpan? MaxAmountOfTimePerWeek { get; set; }
    public int? MaxAverageAmountOfTimePerWeeksAmount { get; set; }
    public TimeSpan? MaxAverageAmountOfTimePerAmountOfWeeks { get; set; }
    public TimeSpan? MaxAmountOfTimePerDay { get; set; }
    public int? MaxAmountOfDaysPerWeek { get; set; }
    public TimeOnly? MaxEndTime { get; set; }
}
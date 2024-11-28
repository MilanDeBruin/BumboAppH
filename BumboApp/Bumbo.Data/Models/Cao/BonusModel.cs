namespace Bumbo.Data.Models.Cao;

public class BonusModel
{
    public DayOfWeek? DayOfWeek { get; set; }
    public TimeOnly? BeginTime { get; set; }
    public TimeOnly? EndTIme { get; set; }
    public double BonusPercentage { get; set; }
}
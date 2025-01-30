namespace Bumbo.Domain.Enums;

public enum CaoSheduleValidatorEnum
{
    Valid,
    TooManyConsecutiveHours,
    NotEnoughBreakTime,
    TooManyDailyHours,
    TooLateEndTime,
    TooManyWeeklyWorkDays,
    TooManyWeeklyHours,
    TooManyAverageWeeklyHoursPerAmountOfWeeks,
    Error
    
}
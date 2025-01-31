using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Cao;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Bumbo.Domain.Services.CAO;

public class CaoScheduleService : ICaoScheduleService
{
    private readonly ICaoRepository _caoRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAvailabilityRepository _availabilityRepository;
    
    public CaoScheduleService(ICaoRepository caoRepository, IScheduleRepository scheduleRepository, IEmployeeRepository employeeRepository, IAvailabilityRepository availabilityRepository)
    {
        _caoRepository = caoRepository;
        _scheduleRepository = scheduleRepository;
        _employeeRepository = employeeRepository;
        _availabilityRepository = availabilityRepository;
    }
    
    public CaoSheduleValidatorEnum ValidateSchedule(WorkSchedule schedule)
    {
        BreakTimeModel breakTimeModel = _caoRepository.GetBreakTime();
        List<WorkHourRestrictionModel> workHourRestrictionModels = _caoRepository.GetWorkHourRestrictions();
        List<WorkSchedule> weeklyWorkSchedule =
            _scheduleRepository.GetWeeklyWorkSchedules(DateOnlyHelper.GetFirstDayOfWeek(schedule.Date),
                schedule.EmployeeId);
        List<WorkSchedule> dailyWorkSchedule = weeklyWorkSchedule.Where(ws => ws.Date == schedule.Date).ToList();
        Employee? employee = _employeeRepository.GetEmployeeByEmployeeId(schedule.EmployeeId);
        int employeeAge = employee != null
            ? new DateTime((DateTime.Today - employee.DateOfBirth.ToDateTime(new TimeOnly())).Ticks).Year - 1
            : 18;


        if (!CheckForConsecutiveHours(breakTimeModel.MaxConsecutiveWorkTime, schedule))
        {
            return CaoSheduleValidatorEnum.TooManyConsecutiveHours;
        }

        if (!CheckForBreakTime(schedule, dailyWorkSchedule, breakTimeModel.MinBreakTime))
        {
            return CaoSheduleValidatorEnum.NotEnoughBreakTime;
        }

        foreach (var restrictionModel in workHourRestrictionModels)
        {
            if (restrictionModel.MaxAge != null && employeeAge > restrictionModel.MaxAge)
            {
                continue;
            }

            if (restrictionModel.MaxAmountOfTimePerWeek != null && !CheckForAmountOfWeeklyHours(schedule, weeklyWorkSchedule, restrictionModel.MaxAmountOfTimePerWeek))
            {
                return CaoSheduleValidatorEnum.TooManyWeeklyHours;
            }

            if (restrictionModel.MaxAverageAmountOfTimePerWeeksAmount != null &&
                restrictionModel.MaxAverageAmountOfTimePerAmountOfWeeks != null && !CheckForAverageAmountOfWeeklyHours(schedule, restrictionModel.MaxAverageAmountOfTimePerAmountOfWeeks, restrictionModel.MaxAverageAmountOfTimePerWeeksAmount))
            {
                return CaoSheduleValidatorEnum.TooManyAverageWeeklyHoursPerAmountOfWeeks;
            }

            SchoolSchedule? schoolSchedule = restrictionModel.MaxAmountOfTimePerDayIncludesSchool == true
                ? _availabilityRepository.GetDailySchoolSchedule(schedule.EmployeeId, schedule.Date.DayOfWeek)
                : null;
            if (restrictionModel.MaxAmountOfTimePerDay != null && !CheckForDailyHours(schedule, dailyWorkSchedule, schoolSchedule, restrictionModel.MaxAmountOfTimePerDay))
            {
                return CaoSheduleValidatorEnum.TooManyDailyHours;
            }

            if (restrictionModel.MaxAmountOfDaysPerWeek != null && !CheckForAmountOfWeeklyWorkdays(schedule, weeklyWorkSchedule, restrictionModel.MaxAmountOfDaysPerWeek))
            {
                return CaoSheduleValidatorEnum.TooManyWeeklyWorkDays;
            }

            if (restrictionModel.MaxEndTime != null && !CheckForEndTime(schedule, restrictionModel.MaxEndTime))
            {
                return CaoSheduleValidatorEnum.TooLateEndTime;
            }
            
        }

        return CaoSheduleValidatorEnum.Valid;
    }

    private bool CheckForConsecutiveHours(TimeSpan maxHours, WorkSchedule schedule)
    {
        TimeSpan amountOfWorkHours = schedule.EndTime - schedule.StartTime;
        if (amountOfWorkHours > maxHours)
        {
            return false;
        }

        return true;
    }

    private bool CheckForBreakTime(WorkSchedule schedule, List<WorkSchedule> dailySchedule, TimeSpan minBreakTime)
    {
        if (dailySchedule.IsNullOrEmpty())
        {
            return true;
        }

        foreach (var subSchedule in dailySchedule)
        {
            if (schedule.EndTime < subSchedule.StartTime && (subSchedule.StartTime-schedule.EndTime) < minBreakTime)
            {
                return false;
            }

            if (schedule.StartTime - (subSchedule.EndTime ) < minBreakTime)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckForDailyHours(WorkSchedule schedule, List<WorkSchedule> dailySchedule, SchoolSchedule? schoolSchedule, TimeSpan? maxWorkHours)
    {
        TimeSpan? workHours = new TimeSpan();
        workHours += schedule.EndTime - schedule.StartTime;

        if (!dailySchedule.IsNullOrEmpty())
        {
            foreach (var workSchedule in dailySchedule)
            {
                workHours += workSchedule.EndTime - workSchedule.StartTime;
            }
        }

        if (schoolSchedule != null)
        {
            SchoolSchedule newSchedule = schoolSchedule;
            workHours += newSchedule.EndTime - schoolSchedule.StartTime;
        }
        
        Console.WriteLine(workHours);
        
        return !(workHours > maxWorkHours);
    }

    private bool CheckForEndTime(WorkSchedule schedule, TimeOnly? maxEndTime)
    {
        return !(schedule.EndTime > maxEndTime);
    }

    private bool CheckForAmountOfWeeklyWorkdays(WorkSchedule schedule, List<WorkSchedule> weeklySchedule, int? maxAmountOfWorkDays)
    {
        if (weeklySchedule.Exists(ws => ws.Date == schedule.Date))
        {
            return true;
        }

        weeklySchedule = weeklySchedule.GroupBy(ws => ws.Date).Select(s => s.First()).ToList();

        if (maxAmountOfWorkDays != null && weeklySchedule.Count >= maxAmountOfWorkDays)
        {
            return false;
        }
        return true;
    }

    private bool CheckForAmountOfWeeklyHours(WorkSchedule schedule, List<WorkSchedule> weeklySchedule, TimeSpan? maxAmountOfWorkHours)
    {
        TimeSpan workTime = schedule.EndTime - schedule.StartTime;

        if (!weeklySchedule.IsNullOrEmpty())
        {
            foreach (var workSchedule in weeklySchedule)
            {
                workTime += workSchedule.EndTime - workSchedule.StartTime;
            }
        }
        
        return !(workTime > maxAmountOfWorkHours);
    }

    private bool CheckForAverageAmountOfWeeklyHours(WorkSchedule schedule, TimeSpan? maxAmountOfAverageWorkHours, int? amountOfWeeks)
    { 
        DateOnly firstDateOfWeeks = schedule.Date.AddDays(7 * ((int)amountOfWeeks - 1));
        List<WorkSchedule> totalWorkSchedule =
            _scheduleRepository.GetAmountOfWeeksWorkSchedule(firstDateOfWeeks, schedule.EmployeeId, (int)amountOfWeeks);

        TimeSpan workTime = schedule.EndTime - schedule.StartTime;
        
        foreach (var workSchedule in totalWorkSchedule)
        {
            workTime += workSchedule.EndTime - workSchedule.StartTime;
        }

        workTime = TimeSpan.FromTicks(workTime.Ticks / (int)amountOfWeeks);

        if (workTime > maxAmountOfAverageWorkHours)
        {
            return false;
        }
        return true;
    }
}
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
        Employee employee = _employeeRepository.GetEmployee(schedule.EmployeeId);
        int employeeAge = new DateTime((DateTime.Today - employee.DateOfBirth.ToDateTime(new TimeOnly())).Ticks).Year - 1;


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

            if (restrictionModel.MaxAmountOfTimePerWeek != null && !CheckForAmountOfWeeklyHours(schedule, weeklyWorkSchedule))
            {
                return CaoSheduleValidatorEnum.TooManyWeeklyHours;
            }

            if (restrictionModel.MaxAverageAmountOfTimePerWeeksAmount != null &&
                restrictionModel.MaxAverageAmountOfTimePerAmountOfWeeks != null && !CheckForAverageAmountOfWeeklyHours(schedule))
            {
                return CaoSheduleValidatorEnum.TooManyAverageWeeklyHoursPerAmountOfWeeks;
            }

            SchoolSchedule? schoolSchedule = restrictionModel.MaxAmountOfTimePerDayIncludesSchool == true
                ? _availabilityRepository.GetDailySchoolSchedule(employee.EmployeeId, schedule.Date.DayOfWeek)
                : null;
            if (restrictionModel.MaxAmountOfTimePerDay != null && !CheckForDailyHours(schedule, dailyWorkSchedule, schoolSchedule, restrictionModel.MaxAmountOfTimePerDay))
            {
                return CaoSheduleValidatorEnum.TooManyDailyHours;
            }

            if (restrictionModel.MaxAmountOfDaysPerWeek != null && !CheckForAmountOfWeeklyWorkdays(schedule, weeklyWorkSchedule))
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

            for (int i = 0; i < dailySchedule.Count; i++)
            {
                workHours += dailySchedule[i].EndTime - dailySchedule[i].StartTime;
            }
        }

        if (schoolSchedule != null)
        {
            SchoolSchedule newSchedule = schoolSchedule;
            workHours += newSchedule.EndTime - schoolSchedule.StartTime;
        }
        
        Console.WriteLine(workHours);

        // workHours = new TimeSpan(12, 1, 0);
        if (workHours > maxWorkHours)
        {
            return false;
        }
        
        return true;
    }

    private bool CheckForEndTime(WorkSchedule schedule, TimeOnly? maxEndTime)
    {
        if (schedule.EndTime > maxEndTime)
        {
            return false;
        }

        return true;
    }

    private bool CheckForAmountOfWeeklyWorkdays(WorkSchedule schedule, List<WorkSchedule> weeklySchedule)
    {
        return true;
    }

    private bool CheckForAmountOfWeeklyHours(WorkSchedule schedule, List<WorkSchedule> weeklySchedule)
    {
        return true;
        
    }

    private bool CheckForAverageAmountOfWeeklyHours(WorkSchedule schedule)
    {
        return true;
    }
}
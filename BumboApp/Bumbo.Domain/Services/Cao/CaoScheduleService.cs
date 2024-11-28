using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Cao;
using Bumbo.Domain.Enums;

namespace Bumbo.Domain.Services.CAO;

public class CaoScheduleService : ICaoScheduleService
{
    private readonly ICaoRepository _caoRepository;
    private readonly IScheduleRepository _scheduleRepository;
    
    public CaoScheduleService(ICaoRepository caoRepository, IScheduleRepository scheduleRepository)
    {
        _caoRepository = caoRepository;
        _scheduleRepository = scheduleRepository;
    }
    
    public CaoSheduleValidatorEnum ValidateSchedule(WorkSchedule schedule)
    {
        BreakTimeModel breakTimeModel = _caoRepository.GetBreakTime();
        List<WorkHourRestrictionModel> workHourRestrictionModels = _caoRepository.GetWorkHourRestrictions();
        
        
        throw new NotImplementedException();
    }

    private bool CheckForConsecutiveHours(WorkSchedule schedule)
    {
        throw new NotImplementedException();
    }

    private bool CheckForBreakTime(WorkSchedule schedule)
    {
        throw new NotImplementedException();
    }

    private bool CheckForDailyHours(WorkSchedule schedule)
    {
        throw new NotImplementedException();
    }

    private bool CheckForEndTime(WorkSchedule schedule)
    {
        throw new NotImplementedException();
        
    }

    private bool CheckForAmountOfWeeklyWorkdays(WorkSchedule schedule)
    {
        throw new NotImplementedException();
    }

    private bool CheckForAmountOfWeeklyHours(WorkSchedule schedule)
    {
        throw new NotImplementedException();
        
    }

    private bool CheckForAverageAmountOfWeeklyHours(WorkSchedule schedule)
    {
        throw new NotImplementedException();
    }
}
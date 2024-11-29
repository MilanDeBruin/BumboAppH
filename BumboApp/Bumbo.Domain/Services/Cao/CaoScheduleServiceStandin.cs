using Bumbo.Data.Models;
using Bumbo.Domain.Enums;

namespace Bumbo.Domain.Services.CAO;

public class CaoScheduleServiceStandin : ICaoScheduleService
{

    public CaoSheduleValidatorEnum ValidateSchedule(WorkSchedule schedule)
    {
        var rnd = new Random();
        return (CaoSheduleValidatorEnum) rnd.Next(Enum.GetNames(typeof(CaoSheduleValidatorEnum)).Length);
    }
    
    public CaoSheduleValidatorEnum ReturnExactEnum(WorkSchedule schedule, CaoSheduleValidatorEnum validatorEnum)
    {
        return validatorEnum;
    }
    
}
using Bumbo.Data.Models;
using Bumbo.Domain.Enums;

namespace Bumbo.Domain.Services.CAO;

public interface ICaoScheduleService
{
    public CaoSheduleValidatorEnum ValidateSchedule(WorkSchedule schedule);
    
    public CaoSheduleValidatorEnum ReturnExactEnum(WorkSchedule schedule, CaoSheduleValidatorEnum validatorEnum)
    {
        throw new NotImplementedException();
    }
}
using Bumbo.Data.Models;
using Bumbo.Domain.Enums;

namespace Bumbo.Domain.Services.CAO;

public interface ICaoScheduleService
{
    public CaoSheduleValidatorEnum ValidateSchedule(WorkSchedule schedule);
}
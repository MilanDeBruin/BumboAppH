using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Bumbo.UnitTests.Cao.HelperClasses;

namespace Bumbo.UnitTests.Cao;

public class CaoEndTimeTest
{
    private ICaoScheduleService _caoScheduleService;

    [SetUp]
    public void Setup()
    {
        _caoScheduleService = new CaoScheduleService(new CaoRepository(), new TestScheduleRepository(), new TestEmployeeRepository(), new TestAvailabilityRepository());
    }

    [Test]
    public void EndTimeBeforeMaxEndTime()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 3,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(18, 30, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));

    }

    [Test]
    public void EndTimeExactlyAtMaxEndTime()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 3,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(19, 0, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }

    [Test]
    public void EndTimeAfterMaxEndTime()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 3,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(19, 1, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.TooLateEndTime));
    }
    
    [Test]
    public void EndTimeBeforeMaxEndTimeAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(18, 30, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));

    }

    [Test]
    public void EndTimeExactlyAtMaxEndTimeAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(19, 0, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }

    [Test]
    public void EndTimeAfterMaxEndTimeAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(19, 1, 0)
        };

        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
}
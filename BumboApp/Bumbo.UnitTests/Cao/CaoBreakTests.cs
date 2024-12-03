using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Bumbo.UnitTests.Cao.HelperClasses;

namespace Bumbo.UnitTests.Cao;

[TestFixture]
public class CaoBreakTests
{
    private ICaoScheduleService _caoScheduleService;

    [SetUp]
    public void Setup()
    {
        _caoScheduleService = new CaoScheduleService(new CaoRepository(), new TestScheduleRepository(), new TestEmployeeRepository(), new TestAvailabilityRepository());
    }

    [Test]
    public void BreakTimeMoreThanMinimumForAdultEmployeeOnSingleDayBeforeSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(10, 0, 0),
            EndTime = new TimeOnly(11, 0, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void BreakTimeMoreThanMinimumForAdultEmployeeOnSingleDayAftereSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(15, 0, 0),
            EndTime = new TimeOnly(16, 0, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void BreakTimeExactlyMinimumForAdultEmployeeOnSingleDayBeforeSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(10, 0, 0),
            EndTime = new TimeOnly(11, 30, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void BreakTimeExactlyMinimumForAdultEmployeeOnSingleDayAftereSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(14, 30, 0),
            EndTime = new TimeOnly(16, 0, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void BreakTimeLessThanMinimumForAdultEmployeeOnSingleDayBeforeSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(10, 0, 0),
            EndTime = new TimeOnly(11, 31, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.NotEnoughBreakTime));
    }
    
    [Test]
    public void BreakTimeLessThanMinimumForAdultEmployeeOnSingleDayAftereSinglePlannedSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(14, 20, 0),
            EndTime = new TimeOnly(16, 0, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.NotEnoughBreakTime));
    }
    
    

}
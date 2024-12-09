using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Bumbo.UnitTests.Cao.HelperClasses;

namespace Bumbo.UnitTests.Cao;

[TestFixture]
public class CaoMaxAmountOfDaysPerWeek
{
    private ICaoScheduleService _caoScheduleService;

    [SetUp]
    public void Setup()
    {
        _caoScheduleService = new CaoScheduleService(new CaoRepository(), new TestScheduleRepository(), new TestEmployeeRepository(), new TestAvailabilityRepository());
    }

    [Test]
    public void AmountOfWeeklyDaysLessThanMaximumAge15()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void AmountOfWeeklyDaysLessThanMaximumAge15DayAlreadyHasSchedule()
    {
        Assert.Ignore();
    }

    [Test]
    public void AmountOfWeeklyDaysExactlyMaximumAge15()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 5,
            Date = new DateOnly(2024, 12, 6),
            StartTime = new TimeOnly(10, 0, 0),
            EndTime = new TimeOnly(11, 0, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
        
    }
    
    [Test]
    public void AmountOfWeeklyDaysExactlyMaximumAge15DayAlreadyHasSchedule()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 5,
            Date = new DateOnly(2024, 12, 9),
            StartTime = new TimeOnly(12, 0, 0),
            EndTime = new TimeOnly(13, 0, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }

    [Test]
    public void AmountOfWeeklyDaysMoreThanMaximumAge15()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 5,
            Date = new DateOnly(2024, 12, 14),
            StartTime = new TimeOnly(12, 0, 0),
            EndTime = new TimeOnly(13, 0, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.TooManyWeeklyWorkDays));
    }
    
    
    [Test]
    public void AmountOfWeeklyDaysLessThanMaximumAdult()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void AmountOfWeeklyDaysLessThanMaximumAdultDayAlreadyHasSchedule()
    {
        Assert.Ignore();
    }

    [Test]
    public void AmountOfWeeklyDaysExactlyMaximumAdult()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void AmountOfWeeklyDaysExactlyMaximumAdultDayAlreadyHasSchedule()
    {
        Assert.Ignore();
    }

    [Test]
    public void AmountOfWeeklyDaysMoreThanMaximumAdult()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void AmountOfWeeklyDaysMoreThanMaximumAdultDayAlreadyHasSchedule()
    {
        Assert.Ignore();
    }
}
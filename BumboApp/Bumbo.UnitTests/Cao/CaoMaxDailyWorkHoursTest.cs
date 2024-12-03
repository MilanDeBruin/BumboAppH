using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Bumbo.UnitTests.Cao.HelperClasses;

namespace Bumbo.UnitTests.Cao;

[TestFixture]
public class CaoMaxDailyWorkHoursTest
{
    private ICaoScheduleService _caoScheduleService;

    [SetUp]
    public void Setup()
    {
        _caoScheduleService = new CaoScheduleService(new CaoRepository(), new TestScheduleRepository(), new TestEmployeeRepository(), new TestAvailabilityRepository());
    }


    [Test]
    public void WorkHoursLessThanMaxHoursForAge15WithoutSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAge15WithoutSchool()
    {
        Assert.Ignore(); 
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAge15WithoutSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursLessThanMaxHoursForAge15WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAge15WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAge15WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursLessThanMaxHoursForAge17WithoutSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAge17WithoutSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAge17WithoutSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursLessThanMaxHoursForAge17WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAge17WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAge17WithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursLessThanMaxHoursForAdultWithoutSchool()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 5),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(21, 0, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAdultWithoutSchool()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 5),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(22, 0, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAdultWithoutSchool()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 2,
            Date = new DateOnly(2024, 12, 5),
            StartTime = new TimeOnly(18, 0, 0),
            EndTime = new TimeOnly(22, 1, 0)
        };
        
        var result = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(result, Is.EqualTo(CaoSheduleValidatorEnum.TooManyDailyHours));
    }
    
    [Test]
    public void WorkHoursLessThanMaxHoursForAdultWithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursExactlyMaxHoursForAdultWithSchool()
    {
        Assert.Ignore();
    }
    
    [Test]
    public void WorkHoursMoreThanMaxHoursForAdultWithSchool()
    {
        Assert.Ignore();
    }
    
    
}
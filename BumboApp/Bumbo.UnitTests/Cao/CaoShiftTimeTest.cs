using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Bumbo.UnitTests.Cao.HelperClasses;

namespace Bumbo.UnitTests.Cao;

[TestFixture]
public class CaoShiftTimeTest
{
    
    private ICaoScheduleService _caoScheduleService;

    [SetUp]
    public void Setup()
    {
        _caoScheduleService = new CaoScheduleService(new CaoRepository(), new TestScheduleRepository(), new TestEmployeeRepository(), new TestAvailabilityRepository());
    }
    
    [Test]
    public void ShiftTimeShorterThanMaximumForEmptyAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(12, 0, 0),
            EndTime = new TimeOnly(14, 0, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }

    [Test]
    public void ShiftTimeExactlyMaximumForEmptyAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(12, 0, 0),
            EndTime = new TimeOnly(16, 30, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.Valid));
    }

    [Test]
    public void ShiftTimeMoreThanMaximumForEmptyAdultEmployee()
    {
        WorkSchedule schedule = new WorkSchedule()
        {
            EmployeeId = 1,
            Date = new DateOnly(2024, 12, 2),
            StartTime = new TimeOnly(12, 0, 0),
            EndTime = new TimeOnly(16, 31, 0)
        };
        CaoSheduleValidatorEnum response = _caoScheduleService.ValidateSchedule(schedule);
        Assert.That(response, Is.EqualTo(CaoSheduleValidatorEnum.TooManyConsecutiveHours));
    }
}
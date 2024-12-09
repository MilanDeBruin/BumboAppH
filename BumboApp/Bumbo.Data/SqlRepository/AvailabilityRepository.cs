using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.SqlRepository;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly BumboDbContext _dbContext;

    public AvailabilityRepository(BumboDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public SchoolSchedule? GetDailySchoolSchedule(int employeeId, DayOfWeek dayOfWeek)
    {
        string weekDay;

        switch (dayOfWeek)
        {
            case DayOfWeek.Monday: weekDay = "Maandag";
                break;
            case DayOfWeek.Tuesday: weekDay = "Dinsdag";
                break;
            case DayOfWeek.Wednesday: weekDay = "Woensdag";
                break;
            case DayOfWeek.Thursday: weekDay = "Donderdag";
                break;
            case DayOfWeek.Friday: weekDay = "Vrijdag";
                break;
            case DayOfWeek.Saturday: weekDay = "Zaterdag";
                break;
            default: weekDay = "Zondag";
                break;
        }

        return _dbContext.SchoolSchedules.FirstOrDefault(s => s.EmployeeId == employeeId && s.Weekday == weekDay);
        
    }

    public TimeOnly GetStoreOpeningHour(int branchId, DateOnly dayOfWeek)
    {
        string weekdayName = dayOfWeek.ToString("dddd", new System.Globalization.CultureInfo("nl-NL"));

        var openingHour = _dbContext.OpeningHours
                                   .Where(o => o.BranchId == branchId && o.Weekday.ToLower() == weekdayName.ToLower())
                                   .Select(o => o.OpeningTime)
                                   .FirstOrDefault();

        if (openingHour == default)
        {
            return new TimeOnly();
            //throw new KeyNotFoundException($"Opening hour for {weekdayName} not found for branch {branchId}");
        }

        return openingHour;
    }

    public TimeOnly GetStoreClosingHour(int branchId, DateOnly dayOfWeek)
    {
        string weekdayName = dayOfWeek.ToString("dddd", new System.Globalization.CultureInfo("nl-NL"));

        var closingHour = _dbContext.OpeningHours
                                   .Where(o => o.BranchId == branchId && o.Weekday.ToLower() == weekdayName.ToLower())
                                   .Select(o => o.ClosingTime)
                                   .FirstOrDefault();

        if (closingHour == default)
        {
            return new TimeOnly();
            //throw new KeyNotFoundException($"Opening hour for {weekdayName} not found for branch {branchId}");
        }

        return closingHour;
    }
    public List<Availability> GetWeekAvailability(int branchId, DateOnly firstDayOfWeek)
    {
        List<string> daysOfWeek = new List<string>();
        for (int i = 0; i < 7; i++)
        {
            daysOfWeek.Add(firstDayOfWeek.AddDays(i).ToString("dddd", new System.Globalization.CultureInfo("nl-NL")));
        }

        // Haal beschikbaarheden op en koppel de Employee-informatie
        List<Availability> weekAvailability = _dbContext.Availabilities
            .Join(_dbContext.Employees,
                  availability => availability.EmployeeId,
                  employee => employee.EmployeeId,
                  (availability, employee) => new { availability, employee })
            .Where(x => daysOfWeek.Contains(x.availability.Weekday) && x.employee.BranchId == branchId)
            .Select(x => new Availability
            {
                EmployeeId = x.availability.EmployeeId,
                Weekday = x.availability.Weekday,
                StartTime = x.availability.StartTime,
                EndTime = x.availability.EndTime,
                Employee = x.employee  // Hier voegen we de Employee toe aan de Availability
            })
            .ToList();

        return weekAvailability;
    }


}

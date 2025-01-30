using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.SqlRepository;

public class ShiftTradeRepository : IShiftTradeRepository
{
    private readonly BumboDbContext _dbContext;
    
    public ShiftTradeRepository(BumboDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool OfferShift(int employee, int branch, DateOnly date, TimeOnly startTime)
    {
        _dbContext.WorkSchedules.First(s =>
                s.EmployeeId == employee && s.BranchId == branch && s.Date == date && s.StartTime == startTime)
            .WorkStatus =
            "Requested";
        try
        {
            _dbContext.SaveChangesAsync().Wait();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool ClaimShift(int employee, int branch, DateOnly date, TimeOnly startTime, int newEmployee)
    {
        var schedule = _dbContext.WorkSchedules.First(s =>
            s.EmployeeId == employee && s.BranchId == branch && s.Date == date && s.StartTime == startTime);
        schedule.WorkStatus = "Default";
        schedule.EmployeeId = newEmployee;
        try
        {
            _dbContext.SaveChangesAsync().Wait();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool AcceptShiftOffer(int employee, int branch, DateOnly date, TimeOnly startTime)
    {
        var shift = _dbContext.WorkSchedules.First(s =>
            s.EmployeeId == employee && s.BranchId == branch && s.Date == date && s.StartTime == startTime);
        shift.WorkStatus = "Accepted";
        try
        {
            _dbContext.SaveChangesAsync().Wait();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool DenyShiftOffer(int employee, int branch, DateOnly date, TimeOnly startTime)
    {
        _dbContext.WorkSchedules.First(s =>
                    s.EmployeeId == employee && s.BranchId == branch && s.Date == date && s.StartTime == startTime)
                .WorkStatus =
            "Denied";
        try
        {
            _dbContext.SaveChangesAsync().Wait();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public List<WorkSchedule> GetShiftClaimRequests(int branch)
    {
        return _dbContext.WorkSchedules.Include(s => s.Employee).Where(s => s.WorkStatus == "Claimed" && s.BranchId == branch).ToList();
    }

    public List<WorkSchedule> GetShiftOffers(int branch)
    {
        return _dbContext.WorkSchedules.Where(s => s.WorkStatus == "Requested").ToList();
    }
}
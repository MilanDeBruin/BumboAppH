using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IShiftTradeRepository
{
    public bool OfferShift(int employee, int branch, DateOnly date, TimeOnly startTime);

    public bool ClaimShift(int employee, int branch, DateOnly date, TimeOnly startTime, int newEmployee);

    public bool AcceptShiftOffer(int employee, int branch, DateOnly date, TimeOnly startTime);

    public bool DenyShiftOffer(int employee, int branch, DateOnly date, TimeOnly startTime);

    /// <summary>
    /// Method returns all offered shifts requests to me assessed by the manager
    /// </summary>
    public List<WorkSchedule> GetShiftClaimRequests(int branch);

    /// <summary>
    /// Method returns all available offered shifts for employees to claim
    /// </summary>
    public List<WorkSchedule> GetShiftOffers(int branch);
}
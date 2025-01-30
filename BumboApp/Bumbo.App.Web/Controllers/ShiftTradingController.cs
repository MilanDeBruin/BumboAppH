using System.Data.Common;
using Bumbo.App.Web.Models.ViewModels.ShiftTrading;
using Bumbo.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers;

[Authorize]
public class ShiftTradingController : Controller
{
    private readonly IShiftTradeRepository _shiftTradeRepository;
    
    public ShiftTradingController(IShiftTradeRepository repository)
    {
        _shiftTradeRepository = repository;
    }

    [Authorize(Roles = "manager")]
    [HttpGet]
    public IActionResult Manager()
    {
        var branchId = int.Parse(User.FindFirst("branch_id")?.Value);
        var shifts = _shiftTradeRepository.GetShiftOfferRequests(branchId);

        var model = new ShiftTradingViewModel();

        foreach (var shift in shifts)
        {
            model.Shifts.Add(new ShiftTradingShiftViewModel()
            {
                BranchId = shift.BranchId,
                Date = shift.Date,
                Department = shift.Department,
                EmployeeName = shift.Employee.FirstName + " " + shift.Employee.Infix + " " + shift.Employee.LastName,
                EmployeeId = shift.EmployeeId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult OfferedShifts()
    {
        var branchId = int.Parse(User.FindFirst("branch_id")?.Value);
        var shifts = _shiftTradeRepository.GetShiftOffers(branchId);

        var model = new ShiftTradingViewModel();

        foreach (var shift in shifts)
        {
            model.Shifts.Add(new ShiftTradingShiftViewModel()
            {
                BranchId = shift.BranchId,
                Date = shift.Date,
                Department = shift.Department,
                EmployeeName = shift.Employee.FirstName + " " + shift.Employee.Infix + " " + shift.Employee.LastName,
                EmployeeId = shift.EmployeeId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            });
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult OfferShift(int employee, int branch, string date, string startTime)
    {
        var localStartTime = TimeOnly.ParseExact(startTime, "HH:mm:ss");
        var localDate = DateOnly.ParseExact(date, "dd-MM-yyyy");

        _shiftTradeRepository.OfferShift(employee, branch, localDate, localStartTime);
        return Redirect("/home");
    }

    [HttpPost]
    public IActionResult ClaimShift(int employee, int branch, string date, string startTime)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    public IActionResult AcceptShiftOffer(int employee, int branch, string date, string startTime)
    {
        var localStartTime = TimeOnly.ParseExact(startTime, "HH:mm:ss");
        var localDate = DateOnly.ParseExact(date, "dd-MM-yyyy");

        _shiftTradeRepository.AcceptShiftOffer(employee, branch, localDate, localStartTime);
        return RedirectToAction("Manager");
    }
    
    [Authorize(Roles = "manager")]
    [HttpPost]
    public IActionResult DenyShiftOffer(int employee, int branch, string date, string startTime)
    {
        var localStartTime = TimeOnly.ParseExact(startTime, "HH:mm:ss");
        var localDate = DateOnly.ParseExact(date, "dd-MM-yyyy");

        _shiftTradeRepository.DenyShiftOffer(employee, branch, localDate, localStartTime);
        return RedirectToAction("Manager");
    }
    
    
}
using System.Data.Common;
using Bumbo.App.Web.Models.ViewModels.ShiftTrading;
using Bumbo.Data.Interfaces;
using Bumbo.Domain.Enums;
using Bumbo.Domain.Services.CAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers;

[Authorize]
public class ShiftTradingController : Controller
{
    private readonly IShiftTradeRepository _shiftTradeRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICaoScheduleService _caoScheduleService;
    
    public ShiftTradingController(IShiftTradeRepository repository, IScheduleRepository scheduleRepository, ICaoScheduleService caoScheduleService)
    {
        _shiftTradeRepository = repository;
        _scheduleRepository = scheduleRepository;
        _caoScheduleService = caoScheduleService;
    }

    [Authorize(Roles = "manager")]
    [HttpGet]
    public IActionResult Manager()
    {
        var branchId = int.Parse(User.FindFirst("branch_id")?.Value);
        var shifts = _shiftTradeRepository.GetShiftClaimRequests(branchId);

        var model = new ShiftTradingViewModel();

        foreach (var shift in shifts)
        {
            model.Shifts.Add(new ShiftTradingShiftViewModel()
            {
                BranchId = shift.BranchId,
                Date = shift.Date,
                Department = shift.Department,
                EmployeeName = shift.Employee.FirstName + " " + shift.Employee.Infix + " " + shift.Employee.LastName,
                ClaimEmployeeName = shift.TradeEmployee.FirstName + " " + shift.TradeEmployee.Infix + " " + shift.TradeEmployee.LastName,
                EmployeeId = shift.EmployeeId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult OfferedShifts(CaoSheduleValidatorEnum? claimStatus)
    {
        var branchId = int.Parse(User.FindFirst("branch_id")?.Value);
        var currectEmployeeId = int.Parse(User.FindFirst("employee_id")?.Value);
        var shifts = _shiftTradeRepository.GetShiftOffers(branchId);

        var model = new EmployeeShiftTradingViewModel();
        model.claimStatus = claimStatus;
        model.CurrentEmployeeId = currectEmployeeId;

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
    public IActionResult ClaimShift(int originalEmployee, int branch, string date, string startTime, int claimEmployee)
    {
        var localStartTime = TimeOnly.ParseExact(startTime, "HH:mm:ss");
        var localDate = DateOnly.ParseExact(date, "dd-MM-yyyy");

        var shift = _scheduleRepository.GetSchedule(originalEmployee, branch, localDate, localStartTime);
        shift.EmployeeId = claimEmployee;

        var result = _caoScheduleService.ValidateSchedule(shift);
        
        if (result != CaoSheduleValidatorEnum.Valid)
        {
            return RedirectToAction("OfferedShifts", new { claimStatus = result });
        }

        var claimResult = _shiftTradeRepository.ClaimShift(originalEmployee, branch, localDate, localStartTime, claimEmployee);

        if (claimResult == false)
        {
            result = CaoSheduleValidatorEnum.Error;
        }
        
        return RedirectToAction("OfferedShifts", new { claimStatus = result });
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
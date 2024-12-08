﻿using Bumbo.App.Web.Models.ViewModels.Leave;
using Bumbo.App.Web.Models.ViewModels.LeaveRequest;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Services.Leaves;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveRepository repo;
        private readonly ILeaveChecker LeaveChecker;
        private readonly IEmployeeRepository empRepo;

        public LeaveController(ILeaveRepository repo, ILeaveChecker lRepo, IEmployeeRepository empRepo)
        {
            this.repo = repo;
            this.LeaveChecker = lRepo;
            this.empRepo = empRepo;
        }
        public IActionResult Index()
        {

            LeaveRequestModel viewModel = new LeaveRequestModel();
            viewModel.start = DateOnly.FromDateTime(DateTime.Now);
            viewModel.end = viewModel.start;

            viewModel.status = "Requested";

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(LeaveRequestModel viewModel)
        {
            int employeeID = 1;
            Leave newRequest = new Leave();
            newRequest.EmployeeId = viewModel.employeeId;
            newRequest.StartDate = viewModel.start;
            newRequest.EndDate = viewModel.end;
            newRequest.LeaveStatus = viewModel.status;

            if (LeaveChecker.startDateHigherThanEndDate(newRequest) && LeaveChecker.checkForOverlap(repo.getAllRequestsOfEmployee(employeeID) , newRequest))
            {
                repo.SetLeaveRequest(newRequest);
                TempData["SuccessMessage"] = $"Verlof is aangevraagd!";
            }
            else
            {
                TempData["FailedMessage"] = $"Verlof is niet aangevraagd!";

            }

            return RedirectToAction("Index");
        }

        public IActionResult MyRequest()
        {
            int employeeID = 1;
            AllLeaveRequestsModel viewModel = new AllLeaveRequestsModel();
            viewModel.myRequests = new List<LeaveRequestModel>(); 

            var requests = repo.getAllRequestsOfEmployee(employeeID);

            foreach (var request in requests)
            {
                LeaveRequestModel model = new LeaveRequestModel(); 
                model.employeeId = employeeID;
                model.start = request.StartDate;
                model.end = request.EndDate;
                model.status = request.LeaveStatus;
                viewModel.myRequests.Add(model);
            }

            return View(viewModel);
        }

        public IActionResult LeaveManagement() 
        {
            AllLeaveRequestsModel viewModel = new AllLeaveRequestsModel();
            viewModel.myRequests = new List<LeaveRequestModel>();

            var requests = repo.getAllRequests();
            foreach (var request in requests)
            {
                LeaveRequestModel model = new LeaveRequestModel();
                model.employeeId = request.EmployeeId;
                model.employeeName = empRepo.FindNameFromId(request.EmployeeId);
                model.start = request.StartDate;
                model.end = request.EndDate;
                model.status = request.LeaveStatus;
                viewModel.myRequests.Add(model);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Approve(int employeeId, DateOnly start)
        {
            TempData["SuccessMessage"] = $"Verlof is goed gekeurd!";
            Leave leave = repo.getLeaveRequest(employeeId,start);
            leave.LeaveStatus = "Accepted";
            repo.updateLeaveStatus(leave);

            return RedirectToAction("LeaveManagement");
        }

        [HttpPost]
        public IActionResult Reject(int employeeId, DateOnly start)
        {
            TempData["FailedMessage"] = $"Verlof is geweigerd!";
            Leave leave = repo.getLeaveRequest(employeeId, start);
            leave.LeaveStatus = "Denied";
            repo.updateLeaveStatus(leave);
            return RedirectToAction("LeaveManagement");
        }
    }
}

using Bumbo.App.Web.Models.ViewModels.Leave;
using Bumbo.App.Web.Models.ViewModels.LeaveRequest;
using Bumbo.Data.Models.LeaveModel;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Services.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;
using System.Collections.Generic;

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
            viewModel.myRequests = repo.getAllRequestsOfEmployee(1);

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
            if (repo.checkStartDateForDuble(newRequest.EmployeeId, newRequest.StartDate))
            {
                TempData["FailedMessage"] = $"Er is al verlof aangevraagd op deze datum!";
                return RedirectToAction("Index");
            }
            if (LeaveChecker.startDateHigherThanEndDate(newRequest) && repo.getOverlap(newRequest.StartDate, newRequest.EndDate, newRequest.EmployeeId))
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

        [HttpGet]
        public IActionResult LeaveManagement(string? firstDayOfWeek)
        {
            AllLeaveRequestsModel viewModel = new AllLeaveRequestsModel();
            viewModel.myRequests = new List<LeaveRequestModel>();
            viewModel.myRequestsList = new List<List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO>>();

            DateTime today = DateTime.Today; ;
            if (firstDayOfWeek != null)
            {
                today = DateTime.Parse(firstDayOfWeek);
            }

            viewModel.startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            viewModel.weekDates = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                viewModel.weekDates.Add(viewModel.startOfWeek.AddDays(i));
            }
            viewModel.endOfWeek = viewModel.weekDates[6];
            DateOnly firstDay = new DateOnly(viewModel.weekDates[0].Year, viewModel.weekDates[0].Month, viewModel.weekDates[0].Day);
            DateOnly lastDay = new DateOnly(viewModel.weekDates[6].Year, viewModel.weekDates[6].Month, viewModel.weekDates[6].Day);


            viewModel.leaves = repo.getAllLeaves(firstDay, lastDay);

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
            List<int> employeeIdList = repo.getEmployeesInLeave();
            List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO> models = new List<LeaveOverviewDTO>();
            models = viewModel.leaves;
            foreach ( int id in employeeIdList)
            {
                List< Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO > LRM = new List<Bumbo.Data.Models.LeaveModel.LeaveOverviewDTO>();
                for(int i = 0; i < models.Count; i++)
                {
                    if (id == models[i].Id)
                    {
                        LRM.Add(models[i]);
                       
                    }
                }
                viewModel.myRequestsList.Add(LRM);
            }

            viewModel.leaves = repo.getAllLeaves(firstDay, lastDay);
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
        public IActionResult Reject(int employeeId, DateOnly start )
        {
            TempData["FailedMessage"] = $"Verlof is geweigerd!";
            Leave leave = repo.getLeaveRequest(employeeId, start);
            leave.LeaveStatus = "Denied";
            repo.updateLeaveStatus(leave);
            return RedirectToAction("LeaveManagement");
        }

      
    }
}

using Bumbo.App.Web.Models.ViewModels.Leave;
using Bumbo.App.Web.Models.ViewModels.LeaveRequest;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveRepository repo;

        public LeaveController(ILeaveRepository repo)
        {
            this.repo = repo;
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

            Leave newRequest = new Leave();
            newRequest.EmployeeId = viewModel.employeeId;
            newRequest.StartDate = viewModel.start;
            newRequest.EndDate = viewModel.end;
            newRequest.LeaveStatus = viewModel.status;


            repo.SetLeaveRequest(newRequest);
            TempData["SuccessMessage"] = $"Verlof is aangevraagd!";
            return RedirectToAction("Index");
        }

        public IActionResult MyRequest()
        {
            int employeeID = 1;
            MyLeaveRequestsModel viewModel = new MyLeaveRequestsModel();
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
            MyLeaveRequestsModel viewModel = new MyLeaveRequestsModel();
            viewModel.myRequests = new List<LeaveRequestModel>();

            var requests = repo.getAllRequests();

            foreach (var request in requests)
            {
                LeaveRequestModel model = new LeaveRequestModel();
                model.employeeId = request.EmployeeId;
                model.start = request.StartDate;
                model.end = request.EndDate;
                model.status = request.LeaveStatus;
                viewModel.myRequests.Add(model);
            }

            return View(viewModel);
        }
    }
}

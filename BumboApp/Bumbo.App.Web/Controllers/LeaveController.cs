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

            return RedirectToAction("Index");
        }


    }
}

using Bumbo.App.Web.Models.ViewModels.LeaveRequest;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {

            LeaveRequestModel viewModel = new LeaveRequestModel();
            viewModel.today = DateTime.Today;
            return View(viewModel);
        }

        //[HttpPost]
        //public IActionResult Index(DateTime start, DateTime end)
        //{

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Index(int employeeId,DateTime start, DateTime end)
        {
            LeaveRequestModel viewModel = new LeaveRequestModel();
            viewModel.employeeId = employeeId;
            viewModel.start = start;
            viewModel.end = end;

            // call naar de database sturen voor een aanvraag
            return RedirectToAction("Index");
        }


    }
}

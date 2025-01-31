using Bumbo.App.Web.Models.ViewModels.WorkedHours;
using Bumbo.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers
{
    public class WorkedHoursController : Controller
    {

        readonly IWorkedHoursRepository _repo;
        public WorkedHoursController(IWorkedHoursRepository repo) {

            _repo = repo;
        }


        public IActionResult Index(int employeeId)
        {
            var shifts = new WorkedHoursViewModel();
            shifts.WorkShifts = _repo.getWorksifts(employeeId);

            return View(shifts);
        }
    }
}

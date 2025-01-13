using Bumbo.App.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.App.Web.Controllers;

public class MonthlyOverviewController : Controller
{

	public IActionResult Index()
	{
		return View(new MonthlyOverviewViewModel());
	}

	[HttpPost]
	public IActionResult GenerateOverview(MonthlyOverviewViewModel model)
	{
		return RedirectToAction("Index", model);
    }

	[HttpPost]
	public IActionResult Save(MonthlyOverviewViewModel model)
	{
		TempData["Message"] = "Monthly overview successfully saved!";
		return RedirectToAction("Index");
	}
}
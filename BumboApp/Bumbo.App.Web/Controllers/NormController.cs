using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace BumboApp.Controllers
{
    [Authorize]
    public class NormController : Controller
    {
        private readonly INormRepository repo;
        public NormController(INormRepository normRepository)
        {
            this.repo = normRepository;
        }

        public IActionResult Index()
        {
            var model = this.repo.GetAll();
            var branchIds = repo.GetAllUniqueBranchIds();
            ViewBag.BranchIds = branchIds;
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Index(String branchId)
        {
            if (branchId != null)
            {
               
                int value = int.Parse(branchId);
                var model = this.repo.GetAllFromBranch(value);
                var branchIds = repo.GetAllUniqueBranchIds();
                ViewBag.BranchIds = branchIds;
                return this.View(model);
            }
            else
                
            {
                var model1 = this.repo.GetAll();
                var branchIds = repo.GetAllUniqueBranchIds();
                ViewBag.BranchIds = branchIds;
                return this.View(model1);
            }
           
        }

        public IActionResult Edit(int? id, int hour, string activity)
        {
            if (id.HasValue && !string.IsNullOrEmpty(activity))
            {
                var norm = repo.GetOne(id.Value);
                norm.SupermarketActivity = activity;
                norm.NormPerHour = hour;
                return View(norm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int id, string activity, Norm norm)
        {
            if (id != norm.BranchId)
            {
                return this.NotFound();
            }
            else
            {
                var model = this.repo.Update(norm);
                return this.RedirectToAction("Index");
            }
            return this.View(norm);
        }

    }
}
using Bumbo.App.Web.Models.ViewModels.Employee;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BumboApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly BumboDbContext _context;

        public EmployeeController(BumboDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            if (employees.Count == 0) return NotFound();

            var model = employees.Select(employee => new EmployeeModel
            {
                employee_id = employee.EmployeeId,
                position = employee.Position,
                first_name = employee.FirstName,
                infix = employee.Infix,
                last_name = employee.LastName,
                date_of_birth = employee.DateOfBirth,
            }).ToList();

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var positions = _context.Positions.Select(p => new { PositionName = p.Position1 }).ToList();
            ViewBag.Positions = new SelectList(positions, "PositionName", "PositionName");

            var branchIDs = _context.Branches.Select(b => new { BranchId = b.BranchId }).ToList();
            ViewBag.BranchIDs = new SelectList(branchIDs, "BranchId", "BranchId");

            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeModel employeeModel)
        {

            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    EmployeeId = employeeModel.employee_id,
                    BranchId = employeeModel.branch_id,
                    Position = employeeModel.position,
                    FirstName = employeeModel.first_name,
                    Infix = employeeModel.infix,
                    LastName = employeeModel.last_name,
                    DateOfBirth = employeeModel.date_of_birth,
                    HouseNumber = employeeModel.house_number,
                    Addition = employeeModel.addition,
                    ZipCode = employeeModel.zip_code,
                    EmailAdres = employeeModel.email_adres,
                    Password = employeeModel.password,
                };

                _context.Employees.Add(employee);
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Medewerker {employeeModel.first_name} is aangemaakt!";

                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }
            }

            var positions = _context.Positions.Select(p => new { PositionName = p.Position1 }).ToList();
            ViewBag.Positions = new SelectList(positions, "PositionName", "PositionName");

            var branchIDs = _context.Branches.Select(b => new { BranchId = b.BranchId }).ToList();
            ViewBag.BranchIDs = new SelectList(branchIDs, "BranchId", "BranchId");

            return View(employeeModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            var employeeModel = new EmployeeModel()
            {
                employee_id = employee.EmployeeId,
                branch_id = employee.BranchId,
                position = employee.Position,
                hiring_date = employee.HiringDate,
                first_name = employee.FirstName,
                infix = employee.Infix,
                last_name = employee.LastName,
                date_of_birth = employee.DateOfBirth,
                house_number = employee.HouseNumber,
                addition = employee.Addition,
                zip_code = employee.ZipCode,
                email_adres = employee.EmailAdres,
                password = employee.Password,
            };

            return View(employeeModel);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            var employeeModel = new EmployeeModel()
            {
                employee_id = employee.EmployeeId,
                branch_id = employee.BranchId,
                position = employee.Position,
                hiring_date = employee.HiringDate,
                first_name = employee.FirstName,
                infix = employee.Infix,
                last_name = employee.LastName,
                date_of_birth = employee.DateOfBirth,
                house_number = employee.HouseNumber,
                addition = employee.Addition,
                zip_code = employee.ZipCode,
                email_adres = employee.EmailAdres,
                password = employee.Password,
            };

            var positions = _context.Positions.Select(p => new { PositionName = p.Position1 }).ToList();
            ViewBag.Positions = new SelectList(positions, "PositionName", "PositionName");

            var branchIDs = _context.Branches.Select(b => new { BranchId = b.BranchId }).ToList();
            ViewBag.BranchIDs = new SelectList(branchIDs, "BranchId", "BranchId");

            return View(employeeModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                var employee = _context.Employees.Find(employeeModel.employee_id);
                if (employee == null) return NotFound();

                employee.EmployeeId = employeeModel.employee_id;
                employee.BranchId = employeeModel.branch_id;
                employee.Position = employeeModel.position;
                employee.HiringDate = employeeModel.hiring_date;
                employee.FirstName = employeeModel.first_name;
                employee.Infix = employeeModel.infix;
                employee.LastName = employeeModel.last_name;
                employee.DateOfBirth = employeeModel.date_of_birth;
                employee.HouseNumber = employeeModel.house_number;
                employee.Addition = employeeModel.addition;
                employee.ZipCode = employeeModel.zip_code;
                employee.EmailAdres = employeeModel.email_adres;
                employee.Password = employeeModel.password;

                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Medewerker {employee.FirstName} is gewijzigd!";

                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error.ErrorMessage);
                }
            }

            var positions = _context.Positions.Select(p => new { PositionName = p.Position1 }).ToList();
            ViewBag.Positions = new SelectList(positions, "PositionName", "PositionName");

            var branchIDs = _context.Branches.Select(b => new { BranchId = b.BranchId }).ToList();
            ViewBag.BranchIDs = new SelectList(branchIDs, "BranchId", "BranchId");

            return View(employeeModel);
        }

        [HttpPost]
        public IActionResult Delete (int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Medewerker {employee.FirstName} is verwijderd!";

            return RedirectToAction("Index");
        }
    }
}

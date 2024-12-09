using Bumbo.App.Web.Models.ViewModels.Employee;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bumbo.App.Web.Controllers
{
    [Authorize]
    public class EmployeeController(BumboDbContext context, IEmployeeRepository employeeRepository) : Controller
    {
        private readonly BumboDbContext _context = context;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public IActionResult Index(int branchId)
        {
            var employees = _employeeRepository.GetEmployees(branchId);

            var model = employees.Select(employee => new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                Position = employee.Position,
                FirstName = employee.FirstName,
                Infix = employee.Infix,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                LaborContract = employee.LaborContract,
            }).ToList();

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Details(int employeeId, int branchId)
        {
            var employee = _employeeRepository.GetEmployee(employeeId);
            if (employee == null) return NotFound();

            var viewModel = new EmployeeViewModel()
            {
                EmployeeId = employee.EmployeeId,
                BranchId = employee.BranchId,
                Position = employee.Position,
                HiringDate = employee.HiringDate,
                FirstName = employee.FirstName,
                Infix = employee.Infix,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                HouseNumber = employee.HouseNumber,
                Addition = employee.Addition,
                ZipCode = employee.ZipCode,
                EmailAdres = employee.EmailAdres,
                Password = employee.Password,
                LaborContract = employee.LaborContract,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Vul dropdown menus (kan niet gebruikmaken van Repository? Convertion problemen met SelectListItem...)
            var viewModel = new EmployeeViewModel
            {
                Branches = [.. _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchId.ToString(),
                })],
                Positions = [.. _context.Positions.Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1
                })],
                LaborContracts = [.. _context.LaborContracts.Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1
                })]
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Hervul dropdown menus
                viewModel.Branches = [.. _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchId.ToString(),
                })];
                viewModel.Positions = [.. _context.Positions.Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1
                })];
                viewModel.LaborContracts = [.. _context.LaborContracts.Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1
                })];

                return View(viewModel);
            }

            var employee = new Employee()
            {
                EmployeeId = viewModel.EmployeeId,
                BranchId = viewModel.BranchId,
                Position = viewModel.Position,
                HiringDate = viewModel.HiringDate,
                FirstName = viewModel.FirstName,
                Infix = viewModel.Infix,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                HouseNumber = viewModel.HouseNumber,
                Addition = viewModel.Addition,
                ZipCode = viewModel.ZipCode,
                EmailAdres = viewModel.EmailAdres,
                Password = viewModel.Password,
                LaborContract = viewModel.LaborContract,
            };

            _employeeRepository.SaveEmployee(employee);
            TempData["SuccessMessage"] = "Medewerker is aangemaakt!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return NotFound();

            var viewModel = new EmployeeViewModel()
            {
                EmployeeId = employee.EmployeeId,
                BranchId = employee.BranchId,
                Position = employee.Position,
                HiringDate = employee.HiringDate,
                FirstName = employee.FirstName,
                Infix = employee.Infix,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                HouseNumber = employee.HouseNumber,
                Addition = employee.Addition,
                ZipCode = employee.ZipCode,
                EmailAdres = employee.EmailAdres,
                Password = employee.Password,
                LaborContract = employee.LaborContract,
                Branches = [.. _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchId.ToString(),
                })],
                Positions = [.. _context.Positions.Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1
                })],
                LaborContracts = [.. _context.LaborContracts.Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1
                })]
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Branches = [.. _context.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchId.ToString(),
                })];
                viewModel.Positions = [.. _context.Positions.Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1
                })];
                viewModel.LaborContracts = [.. _context.LaborContracts.Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1
                })];

                return View(viewModel);
            }

            var employee = new Employee
            {
                EmployeeId = viewModel.EmployeeId,
                BranchId = viewModel.BranchId,
                Position = viewModel.Position,
                HiringDate = viewModel.HiringDate,
                FirstName = viewModel.FirstName,
                Infix = viewModel.Infix,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                HouseNumber = viewModel.HouseNumber,
                Addition = viewModel.Addition,
                ZipCode = viewModel.ZipCode,
                EmailAdres = viewModel.EmailAdres,
                Password = viewModel.Password,
                LaborContract = viewModel.LaborContract
            };

            if (!_employeeRepository.UpdateEmployee(employee))
            {
                TempData["ErrorMessage"] = "Medewerker kon niet worden gewijzigd!";
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Medewerker is gewijzigd!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int employeeId) // Werkt nog niet als Employee een Availability heeft. EmployeeId wordt op null gezet voor de Availability, maar dat kan niet?
        {
            if (!_employeeRepository.DeleteEmployee(employeeId))
            {
                TempData["ErrorMessage"] = "Medewerker kan niet worden verwijderd omdat deze nog gekoppeld is aan een rooster of beschikbaarheid!";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = $"Medewerker is verwijderd!";
            return RedirectToAction("Index");
        }
    }
}

using Bumbo.App.Web.Models.ViewModels.Employee;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Bumbo.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bumbo.App.Web.Controllers
{
    [Authorize(Roles="manager")]
    public class EmployeeController(BumboDbContext context,
        IEmployeeRepository employeeRepository, IBranchRepository branchRepository, 
        IPositionRepository positionRepository, ILaborContractRepository laborContractRepository
        ) : Controller
    {
        private readonly BumboDbContext _context = context;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly IPositionRepository _positionRepository = positionRepository;
        private readonly ILaborContractRepository _laborContractRepository = laborContractRepository;
        
        public IActionResult Index(int branchId)
        {
            var employees = _employeeRepository.GetAllEmployeesByBranchId(branchId);

            var model = employees.Select(employee => new EmployeeCreateViewModel
            {
                UserId = employee.UserId,
                EmployeeId = employee.EmployeeId,
                BranchId = employee.BranchId,
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
        public IActionResult Details(int employeeId)
        {
            var employee = _employeeRepository.GetEmployeeByEmployeeId(employeeId);
            if (employee == null) return NotFound();
            
            var employeeEmail = _employeeRepository.FindEmailFromUserId(employee.UserId);

            var viewModel = new EmployeeCreateViewModel
            {
                UserId = employee.UserId,
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
                EmailAddress = employeeEmail ?? string.Empty,
                LaborContract = employee.LaborContract,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int branchId)
        {
            var employeeViewModel = new EmployeeCreateViewModel
            {
                BranchId = branchId,
                Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                }),
                Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                }),
                LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1,
                })
            };

            return View(employeeViewModel);
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel createViewModel)
        {
            if (!ModelState.IsValid || createViewModel.HiringDate < createViewModel.DateOfBirth)
            {
                if (createViewModel.HiringDate < createViewModel.DateOfBirth)
                {
                    ModelState.AddModelError(
                        nameof(createViewModel.HiringDate),
                        "Startdatum contract kan niet eerder zijn dan geboortedatum"
                    );
                    ModelState.AddModelError(
                        nameof(createViewModel.DateOfBirth),
                        "Geboortedatum kan niet later zijn dan startdatum contract"
                    );
                }

                createViewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                createViewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                createViewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });

                return View(createViewModel);
            }
            
            var employee = new Employee
            {
                EmployeeId = createViewModel.EmployeeId,
                BranchId = createViewModel.BranchId,
                Position = createViewModel.Position,
                HiringDate = createViewModel.HiringDate,
                FirstName = createViewModel.FirstName,
                Infix = createViewModel.Infix,
                LastName = createViewModel.LastName,
                DateOfBirth = createViewModel.DateOfBirth,
                HouseNumber = createViewModel.HouseNumber,
                Addition = createViewModel.Addition,
                ZipCode = createViewModel.ZipCode,
                LaborContract = createViewModel.LaborContract,
            };

            _employeeRepository.AddEmployee(employee, createViewModel.EmailAddress, createViewModel.Password, RoleEnum.Employee);
            TempData["SuccessMessage"] = "Medewerker is aangemaakt!";
            return RedirectToAction("Index", new { branchId = createViewModel.BranchId });
        }

        [HttpGet]
        public IActionResult Edit(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return NotFound();
            
            var employeeEmail = _employeeRepository.FindEmailFromUserId(employee.UserId);

            var viewModel = new EmployeeUpdateViewModel()
            {
                UserId = employee.UserId,
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
                EmailAddress = employeeEmail ?? string.Empty,
                LaborContract = employee.LaborContract,
                Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                }),
                Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                }),
                LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc => new SelectListItem
                {
                    Value = lc.LaborContract1,
                    Text = lc.LaborContract1,
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeUpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"ModelState Error - {error.Key}: {err.ErrorMessage}");
                    }
                }
                updateViewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                updateViewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                updateViewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });

                return View(updateViewModel);
            }

            if (updateViewModel.HiringDate < updateViewModel.DateOfBirth)
            {
                ModelState.AddModelError(
                    nameof(updateViewModel.HiringDate),
                    "Startdatum contract kan niet eerder zijn dan geboortedatum"
                );
                ModelState.AddModelError(
                    nameof(updateViewModel.DateOfBirth),
                    "Geboortedatum kan niet later zijn dan startdatum contract"
                );

                updateViewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                updateViewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                updateViewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });
                
                return View(updateViewModel);
            }
            
            var employee = new Employee
            {
                EmployeeId = updateViewModel.EmployeeId,
                BranchId = updateViewModel.BranchId,
                HiringDate = updateViewModel.HiringDate,
                FirstName = updateViewModel.FirstName,
                Infix = updateViewModel.Infix,
                LastName = updateViewModel.LastName,
                DateOfBirth = updateViewModel.DateOfBirth,
                HouseNumber = updateViewModel.HouseNumber,
                Addition = updateViewModel.Addition,
                ZipCode = updateViewModel.ZipCode,
                LaborContract = updateViewModel.LaborContract
            };

            if (!_employeeRepository.UpdateEmployee(employee, updateViewModel.EmailAddress, updateViewModel.NewPassword))
            {
                TempData["ErrorMessage"] = "Medewerker kon niet worden gewijzigd!";
                return View(updateViewModel);
            }

            TempData["SuccessMessage"] = "Medewerker is gewijzigd!";
            return RedirectToAction("Index", new { branchId = updateViewModel.BranchId });
        }

        [HttpPost]
        public IActionResult Delete(int employeeId)
        {
            var employee = _employeeRepository.GetEmployeeByEmployeeId(employeeId);
            if (employee == null) return NotFound();
            var branchId = employee.BranchId;

            if (!_employeeRepository.DeleteEmployee(employeeId))
            {
                TempData["ErrorMessage"] = "Medewerker kan niet worden verwijderd omdat deze nog gekoppeld is aan een rooster of beschikbaarheid!";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = $"Medewerker is verwijderd!";
            return RedirectToAction("Index", new { branchId });
        }
    }
}

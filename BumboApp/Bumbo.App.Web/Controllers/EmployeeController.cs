﻿using Bumbo.App.Web.Models.ViewModels.Employee;
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

            var model = employees.Select(employee => new EmployeeViewModel
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

            var viewModel = new EmployeeViewModel
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
                LaborContract = employee.LaborContract,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int branchId)
        {
            var employeeViewModel = new EmployeeViewModel
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
        public IActionResult Create(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel.HiringDate < viewModel.DateOfBirth)
            {
                if (viewModel.HiringDate < viewModel.DateOfBirth)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.HiringDate),
                        "Startdatum contract kan niet eerder zijn dan geboortedatum"
                    );
                    ModelState.AddModelError(
                        nameof(viewModel.DateOfBirth),
                        "Geboortedatum kan niet later zijn dan startdatum contract"
                    );
                }

                viewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                viewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                viewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });

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
                LaborContract = viewModel.LaborContract,
            };

            _employeeRepository.SaveEmployee(employee, viewModel.EmailAdres, viewModel.Password, RoleEnum.Employee);
            TempData["SuccessMessage"] = "Medewerker is aangemaakt!";
            return RedirectToAction("Index", new { branchId = viewModel.BranchId });
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
                // EmailAdres = employee.EmailAdres, TODO: Remove?
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
        public IActionResult Edit(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                viewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                viewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });

                return View(viewModel);
            }

            if (viewModel.HiringDate < viewModel.DateOfBirth)
            {
                ModelState.AddModelError(
                    nameof(viewModel.HiringDate),
                    "Startdatum contract kan niet eerder zijn dan geboortedatum"
                );
                ModelState.AddModelError(
                    nameof(viewModel.DateOfBirth),
                    "Geboortedatum kan niet later zijn dan startdatum contract"
                );

                viewModel.Branches = _branchRepository.GetAllBranches().Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.ZipCode,
                });
                viewModel.Positions = _positionRepository.GetAllPositions().Select(p => new SelectListItem
                {
                    Value = p.Position1,
                    Text = p.Position1,
                });
                viewModel.LaborContracts = _laborContractRepository.GetAllLaborContracts().Select(lc =>
                    new SelectListItem
                    {
                        Value = lc.LaborContract1,
                        Text = lc.LaborContract1,
                    });
                
                return View(viewModel);
            }
            
            var employee = new Employee
            {
                EmployeeId = viewModel.EmployeeId,
                BranchId = viewModel.BranchId,
                HiringDate = viewModel.HiringDate,
                FirstName = viewModel.FirstName,
                Infix = viewModel.Infix,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                HouseNumber = viewModel.HouseNumber,
                Addition = viewModel.Addition,
                ZipCode = viewModel.ZipCode,
                LaborContract = viewModel.LaborContract
            };

            if (!_employeeRepository.UpdateEmployee(employee, viewModel.EmailAdres, viewModel.Password))
            {
                TempData["ErrorMessage"] = "Medewerker kon niet worden gewijzigd!";
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Medewerker is gewijzigd!";
            return RedirectToAction("Index", new { branchId = viewModel.BranchId });
        }

        [HttpPost]
        public IActionResult Delete(int employeeId) // Werkt nog niet als Employee een Availability heeft. EmployeeId wordt op null gezet voor de Availability, maar dat kan niet?
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

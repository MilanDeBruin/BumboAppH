using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;

namespace Bumbo.App.Web.Models.ViewModels.Employee;

public class EmployeeViewModel
{
    [Key]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Branch ID is verplicht")]
    public int BranchId { get; set; }
    
    [Required(ErrorMessage = "Afdeling is verplicht")]
    public string Position { get; set; }

    [Required(ErrorMessage = "Startdatum van contract is verplicht")]
    public DateOnly HiringDate { get; set; }

    [Required(ErrorMessage = "Voornaam is verplicht")]
    [StringLength(50, ErrorMessage = "Voornaam moet korter dan 50 letters zijn")]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string? Infix { get; set; }

    [Required(ErrorMessage = "Achternaam is verplicht")]
    [StringLength(50, ErrorMessage = "Achternaam moet korter dan 50 letters zijn")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Geboortedatum is verplicht")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Huisnummer is verplicht")]
    [Range(0, 9999, ErrorMessage = "Huisnummer moet tussen 0 en 9999 zijn")]
    public int HouseNumber { get; set; }

    [StringLength(3, ErrorMessage = "Toevoeging moet korter dan 3 nummers zijn")]
    [RegularExpression(@"^[a-zA-Z0-9]{1,3}$", ErrorMessage = "Toevoeging moet 1 tot 3 alfanumerieke tekens bevatten")]
    public string? Addition { get; set; }

    [Required(ErrorMessage = "Postcode is verplicht")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Postcode moet voldoen aan het Nederlandse formaat")]
    [RegularExpression(@"^\d{4}[A-Za-z]{2}$", ErrorMessage = "Postcode moet het formaat '1234AB' aanhouden.")]
    public string ZipCode { get; set; }

    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [StringLength(254, ErrorMessage = "E-mailadres moet korter dan 254 tekens zijn")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "E-mailadres moet een geldige formaat aanhouden")]
    public string EmailAdres { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht")]
    [StringLength(50, ErrorMessage = "Wachtwoord moet korter dan 50 tekens zijn")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Contracttype is verplicht")]
    public string LaborContract { get; set; }
    
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public IEnumerable<SelectListItem>? Branches { get; set; }
    public IEnumerable<SelectListItem>? Positions { get; set; }
    public IEnumerable<SelectListItem>? LaborContracts { get; set; }
}

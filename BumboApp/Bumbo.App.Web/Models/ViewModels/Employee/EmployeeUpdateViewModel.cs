using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.Employee;

public class EmployeeUpdateViewModel
{
    [Key]
    public int EmployeeId { get; init; }

    [Required(ErrorMessage = "Branch ID is verplicht")]
    public int BranchId { get; init; }
    
    [Required(ErrorMessage = "Afdeling is verplicht")]
    public string Position { get; init; }

    [Required(ErrorMessage = "Startdatum van contract is verplicht")]
    public DateOnly HiringDate { get; init; }

    [Required(ErrorMessage = "Voornaam is verplicht")]
    [StringLength(50, ErrorMessage = "Voornaam moet korter dan 50 letters zijn")]
    public string FirstName { get; init; }

    [StringLength(50)]
    public string? Infix { get; init; }

    [Required(ErrorMessage = "Achternaam is verplicht")]
    [StringLength(50, ErrorMessage = "Achternaam moet korter dan 50 letters zijn")]
    public string LastName { get; init; }

    [Required(ErrorMessage = "Geboortedatum is verplicht")]
    public DateOnly DateOfBirth { get; init; }

    [Required(ErrorMessage = "Huisnummer is verplicht")]
    [Range(0, 9999, ErrorMessage = "Huisnummer moet tussen 0 en 9999 zijn")]
    public int? HouseNumber { get; init; }

    [StringLength(3, ErrorMessage = "Toevoeging moet korter dan 3 nummers zijn")]
    [RegularExpression(@"^[a-zA-Z0-9]{1,3}$", ErrorMessage = "Toevoeging moet 1 tot 3 alfanumerieke tekens bevatten")]
    public string? Addition { get; init; }

    [Required(ErrorMessage = "Postcode is verplicht")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Postcode moet voldoen aan het Nederlandse formaat")]
    [RegularExpression(@"^\d{4}[A-Za-z]{2}$", ErrorMessage = "Postcode moet het formaat '1234AB' aanhouden.")]
    public string ZipCode { get; init; }

    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [StringLength(254, ErrorMessage = "E-mailadres moet korter dan 254 tekens zijn")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "E-mailadres moet een geldige formaat aanhouden")]
    public string EmailAddress { get; init; }

    [StringLength(50, ErrorMessage = "Wachtwoord moet korter dan 50 tekens zijn")]
    public string? NewPassword { get; init; }
    
    [Required(ErrorMessage = "Contracttype is verplicht")]
    public string LaborContract { get; init; }
    
    public string? UserId { get; init; }
    public IEnumerable<SelectListItem>? Branches { get; set; }
    public IEnumerable<SelectListItem>? Positions { get; set; }
    public IEnumerable<SelectListItem>? LaborContracts { get; set; }
}

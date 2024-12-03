using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.Employee;

public class EmployeeViewModel
{
    [Key]
    public int employee_id { get; set; }

    [Required(ErrorMessage = "Branch ID is verplicht.")]
    public int branch_id { get; set; }
    
    [Required(ErrorMessage = "Afdeling is verplicht.")]
    public string position { get; set; }

    [Required(ErrorMessage = "Startdatum van contract is verplicht.")]
    public DateOnly hiring_date { get; set; }

    [Required(ErrorMessage = "Voornaam is verplicht.")]
    [StringLength(50, ErrorMessage = "Voornaam moet korter dan 50 letters zijn.")]
    public string first_name { get; set; }

    [StringLength(50)]
    public string? infix { get; set; }

    [Required(ErrorMessage = "Achternaam is verplicht.")]
    [StringLength(50, ErrorMessage = "Voornaam moet korter dan 50 letters zijn.")]
    public string last_name { get; set; }

    [Required(ErrorMessage = "Geboortedatum is verplicht.")]
    public DateOnly date_of_birth { get; set; }

    [Required(ErrorMessage = "Huisnummer is verplicht.")]
    [Range(0, 9999, ErrorMessage = "Huisnummer moet tussen 0 en 9999 zijn.")]
    public int house_number { get; set; }

    [StringLength(3, ErrorMessage = "Toevoeging moet korter dan 3 nummers zijn.")]
    public string? addition { get; set; }

    [Required(ErrorMessage = "Postcode is verplicht.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Postcode moet uit 6 tekens bestaan.")]
    public string zip_code { get; set; }

    [Required(ErrorMessage = "E-mailadres is verplicht.")]
    [StringLength(254, ErrorMessage = "E-mailadres moet korter dan 254 tekens zijn.")]
    public string email_adres { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    [StringLength(50, ErrorMessage = "Wachtwoord moet korter dan 50 tekens zijn.")]
    public string password { get; set; }
}

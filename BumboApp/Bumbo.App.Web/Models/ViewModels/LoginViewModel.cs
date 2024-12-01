using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.Forecast;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
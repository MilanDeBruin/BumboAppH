using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.Employee;

public class EmployeeViewModel
{
    public int employee_id { get; set; }
    public int branch_id { get; set; }
    public string position { get; set; }
    public DateOnly hiring_date { get; set; }
    public string first_name { get; set; }
    public string? infix { get; set; }
    public string last_name { get; set; }
    public DateOnly date_of_birth { get; set; }
    public int house_number { get; set; }
    public string? addition { get; set; }
    public string zip_code { get; set; }
    public string email_adres { get; set; }
    public string password { get; set; }
    public string labor_contract { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("employee")]
public partial class Employee
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    [Column("branch_id")]
    public int BranchId { get; set; }

    [Column("hiring_date")]
    public DateOnly HiringDate { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Column("infix")]
    [StringLength(50)]
    public string? Infix { get; set; }

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("house_number")]
    public int HouseNumber { get; set; }

    [Column("addition")]
    [StringLength(3)]
    public string? Addition { get; set; }

    [Column("zip_code")]
    [StringLength(20)]
    public string ZipCode { get; set; } = null!;

    [Column("labor_contract")]
    [StringLength(50)]
    public string LaborContract { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    [ForeignKey("BranchId")]
    [InverseProperty("Employees")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("LaborContract")]
    [InverseProperty("Employees")]
    public virtual LaborContract LaborContractNavigation { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    [InverseProperty("Employee")]
    public virtual ICollection<SchoolSchedule> SchoolSchedules { get; set; } = new List<SchoolSchedule>();

    [InverseProperty("Employee")]
    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();

    [ForeignKey("EmployeeId")]
    [InverseProperty("Employees")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("department")]
public partial class Department
{
    [Key]
    [Column("department")]
    [StringLength(5)]
    public string Department1 { get; set; } = null!;

    [InverseProperty("DepartmentNavigation")]
    public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();

    [InverseProperty("DepartmentNavigation")]
    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();

    [ForeignKey("Department")]
    [InverseProperty("Departments")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("Date", "BranchId", "Department")]
[Table("forecast")]
public partial class Forecast
{
    [Key]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Key]
    [Column("department")]
    [StringLength(5)]
    public string Department { get; set; } = null!;

    [Column("man_hours")]
    public int ManHours { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Forecasts")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("Department")]
    [InverseProperty("Forecasts")]
    public virtual Department DepartmentNavigation { get; set; } = null!;
}

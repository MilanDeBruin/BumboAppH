using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("EmployeeId", "StartDate")]
[Table("leave")]
public partial class Leave
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("end_date")]
    public DateOnly EndDate { get; set; }

    [Column("leave_status")]
    [StringLength(50)]
    public string LeaveStatus { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("Leaves")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("LeaveStatus")]
    [InverseProperty("Leaves")]
    public virtual LeaveStatus LeaveStatusNavigation { get; set; } = null!;
}

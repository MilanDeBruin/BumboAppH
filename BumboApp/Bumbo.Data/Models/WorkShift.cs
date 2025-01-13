using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("EmployeeId", "StartTime")]
[Table("work_shift")]
public partial class WorkShift
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("start_time", TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column("end_time", TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("WorkShifts")]
    public virtual Employee Employee { get; set; } = null!;
}

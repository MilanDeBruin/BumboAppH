using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("EmployeeId", "Weekday")]
[Table("availability")]
public partial class Availability
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("weekday")]
    [StringLength(9)]
    public string Weekday { get; set; } = null!;

    [Column("start_time")]
    [Precision(0)]
    public TimeOnly? StartTime { get; set; }

    [Column("end_time")]
    [Precision(0)]
    public TimeOnly? EndTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Availabilities")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("Weekday")]
    [InverseProperty("Availabilities")]
    public virtual Weekday WeekdayNavigation { get; set; } = null!;
}

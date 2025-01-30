using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("EmployeeId", "Date", "BranchId")]
[Table("work_schedule")]
public partial class WorkSchedule
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Column("start_time")]
    [Precision(0)]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    [Precision(0)]
    public TimeOnly EndTime { get; set; }

    [Column("department")]
    [StringLength(5)]
    public string Department { get; set; } = null!;

    [Column("work_status")]
    [StringLength(50)]
    public string WorkStatus { get; set; } = null!;

    [Column("is_sick")]
    public bool IsSick { get; set; }

    [Column("concept")]
    public bool Concept { get; set; }

    [Column("trade_employee")]
    public bool TradeEmployee { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("WorkSchedules")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("Department")]
    [InverseProperty("WorkSchedules")]
    public virtual Department DepartmentNavigation { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("WorkSchedules")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("WorkStatus")]
    [InverseProperty("WorkSchedules")]
    public virtual WorkStatus WorkStatusNavigation { get; set; } = null!;
}

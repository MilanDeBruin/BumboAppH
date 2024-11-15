using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("BranchId", "Weekday")]
[Table("opening_hours")]
public partial class OpeningHour
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Key]
    [Column("weekday")]
    [StringLength(9)]
    public string Weekday { get; set; } = null!;

    [Column("opening_time")]
    public TimeOnly OpeningTime { get; set; }

    [Column("closing_time")]
    public TimeOnly ClosingTime { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("OpeningHours")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("Weekday")]
    [InverseProperty("OpeningHours")]
    public virtual Weekday WeekdayNavigation { get; set; } = null!;
}

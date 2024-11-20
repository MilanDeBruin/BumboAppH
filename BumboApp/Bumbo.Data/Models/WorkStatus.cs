using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("work_status")]
public partial class WorkStatus
{
    [Key]
    [Column("work_status")]
    [StringLength(50)]
    public string WorkStatus1 { get; set; } = null!;

    [InverseProperty("WorkStatusNavigation")]
    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();
}

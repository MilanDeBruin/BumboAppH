using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("leave_status")]
public partial class LeaveStatus
{
    [Key]
    [Column("leave_status")]
    [StringLength(50)]
    public string LeaveStatus1 { get; set; } = null!;

    [InverseProperty("LeaveStatusNavigation")]
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
}

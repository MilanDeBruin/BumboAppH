using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("position")]
public partial class Position
{
    [Key]
    [Column("position")]
    [StringLength(50)]
    public string Position1 { get; set; } = null!;

    [InverseProperty("PositionNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

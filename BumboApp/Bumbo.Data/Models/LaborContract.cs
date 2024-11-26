using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("labor_contract")]
public partial class LaborContract
{
    [Key]
    [Column("labor_contract")]
    [StringLength(50)]
    public string LaborContract1 { get; set; } = null!;

    [InverseProperty("LaborContractNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("supermarket_activity")]
public partial class SupermarketActivity
{
    [Key]
    [Column("supermarket_activity")]
    [StringLength(50)]
    public string SupermarketActivity1 { get; set; } = null!;

    [InverseProperty("SupermarketActivityNavigation")]
    public virtual ICollection<Norm> Norms { get; set; } = new List<Norm>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("BranchId", "SupermarketActivity")]
[Table("norm")]
public partial class Norm
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Key]
    [Column("supermarket_activity")]
    [StringLength(50)]
    public string SupermarketActivity { get; set; } = null!;

    [Column("norm_per_hour")]
    public int NormPerHour { get; set; }

    [Column("unit")]
    [StringLength(50)]
    public string Unit { get; set; } = null!;

    [ForeignKey("BranchId")]
    [InverseProperty("Norms")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("SupermarketActivity")]
    [InverseProperty("Norms")]
    public virtual SupermarketActivity SupermarketActivityNavigation { get; set; } = null!;
}

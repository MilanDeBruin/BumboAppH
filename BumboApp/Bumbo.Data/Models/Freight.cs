using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("BranchId", "Date")]
[Table("freight")]
public partial class Freight
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Key]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Freights")]
    public virtual Branch Branch { get; set; } = null!;
}

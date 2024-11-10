using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[PrimaryKey("BranchId", "DateTime", "Amount")]
[Table("store_traffic")]
public partial class StoreTraffic
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Key]
    [Column("date_time", TypeName = "datetime")]
    public DateTime DateTime { get; set; }

    [Key]
    [Column("amount")]
    public int Amount { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("StoreTraffics")]
    public virtual Branch Branch { get; set; } = null!;
}

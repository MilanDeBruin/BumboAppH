using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("weekday")]
public partial class Weekday
{
    [Key]
    [Column("weekday")]
    [StringLength(8)]
    public string Weekday1 { get; set; } = null!;

    [InverseProperty("WeekdayNavigation")]
    public virtual ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
}

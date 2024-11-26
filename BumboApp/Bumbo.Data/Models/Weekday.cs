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
    [StringLength(9)]
    public string Weekday1 { get; set; } = null!;

    [InverseProperty("WeekdayNavigation")]
    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    [InverseProperty("WeekdayNavigation")]
    public virtual ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();

    [InverseProperty("WeekdayNavigation")]
    public virtual ICollection<SchoolSchedule> SchoolSchedules { get; set; } = new List<SchoolSchedule>();
}

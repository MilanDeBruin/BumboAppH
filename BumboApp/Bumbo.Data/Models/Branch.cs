using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Models;

[Table("branch")]
public partial class Branch
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Column("house_number")]
    public int HouseNumber { get; set; }

    [Column("addition")]
    [StringLength(3)]
    public string? Addition { get; set; }

    [Column("zip_code")]
    [StringLength(20)]
    public string ZipCode { get; set; } = null!;

    [Column("country_code")]
    [StringLength(3)]
    public string CountryCode { get; set; } = null!;

    [Column("latitude", TypeName = "decimal(8, 6)")]
    public decimal Latitude { get; set; }

    [Column("longitude", TypeName = "decimal(9, 6)")]
    public decimal Longitude { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Branch")]
    public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();

    [InverseProperty("Branch")]
    public virtual ICollection<Freight> Freights { get; set; } = new List<Freight>();

    [InverseProperty("Branch")]
    public virtual ICollection<Norm> Norms { get; set; } = new List<Norm>();

    [InverseProperty("Branch")]
    public virtual ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();

    [InverseProperty("Branch")]
    public virtual ICollection<StoreTraffic> StoreTraffics { get; set; } = new List<StoreTraffic>();

    [InverseProperty("Branch")]
    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();
}

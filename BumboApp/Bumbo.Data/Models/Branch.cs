using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [StringLength(6)]
    public string ZipCode { get; set; } = null!;

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
}

using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bumbo.Data.Context;

public partial class BumboDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public BumboDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public BumboDbContext(DbContextOptions<BumboDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Forecast> Forecasts { get; set; }

    public virtual DbSet<Freight> Freights { get; set; }

    public virtual DbSet<Norm> Norms { get; set; }

    public virtual DbSet<OpeningHour> OpeningHours { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<StoreTraffic> StoreTraffics { get; set; }

    public virtual DbSet<SupermarketActivity> SupermarketActivities { get; set; }

    public virtual DbSet<Weekday> Weekdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Bumbo"));
            optionsBuilder.UseSqlServer("Server = .; Database = Bumbo; Integrated Security = True; TrustServerCertificate = True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_branch");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_position");

            entity.HasMany(d => d.Departments).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeInDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("Department")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_employee_in_department_department"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_employee_in_department_employee"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "Department");
                        j.ToTable("employee_in_department");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                        j.IndexerProperty<string>("Department")
                            .HasMaxLength(5)
                            .HasColumnName("department");
                    });
        });

        modelBuilder.Entity<Forecast>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Forecasts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_forecast_branch");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Forecasts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_forecast_department");
        });

        modelBuilder.Entity<Freight>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Freights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_freight_branch");
        });

        modelBuilder.Entity<Norm>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Norms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_norm_branch");

            entity.HasOne(d => d.SupermarketActivityNavigation).WithMany(p => p.Norms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_norm_activity");
        });

        modelBuilder.Entity<OpeningHour>(entity =>
        {
            entity.HasKey(e => new { e.BranchId, e.Weekday }).HasName("PK_opening_hours_1");

            entity.HasOne(d => d.Branch).WithMany(p => p.OpeningHours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_opening_hours_branch");

            entity.HasOne(d => d.WeekdayNavigation).WithMany(p => p.OpeningHours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_opening_hours_weekday");
        });

        modelBuilder.Entity<StoreTraffic>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.StoreTraffics)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_store_traffic_branch");
        });

        modelBuilder.Entity<SupermarketActivity>(entity =>
        {
            entity.HasKey(e => e.SupermarketActivity1).HasName("PK_activity");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

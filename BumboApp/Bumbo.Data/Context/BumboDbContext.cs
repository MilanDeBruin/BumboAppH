using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Context;

public partial class BumboDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Forecast> Forecasts { get; set; }

    public virtual DbSet<Freight> Freights { get; set; }

    public virtual DbSet<LaborContract> LaborContracts { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveStatus> LeaveStatuses { get; set; }

    public virtual DbSet<Norm> Norms { get; set; }

    public virtual DbSet<OpeningHour> OpeningHours { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<SchoolSchedule> SchoolSchedules { get; set; }

    public virtual DbSet<StoreTraffic> StoreTraffics { get; set; }

    public virtual DbSet<SupermarketActivity> SupermarketActivities { get; set; }

    public virtual DbSet<Weekday> Weekdays { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    public virtual DbSet<WorkStatus> WorkStatuses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
        
        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });
        
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.Availabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_availability_employee");

            entity.HasOne(d => d.WeekdayNavigation).WithMany(p => p.Availabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_availability_weekday");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_branch");

            entity.HasOne(d => d.LaborContractNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_labor_contract");

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

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_leave_employee");

            entity.HasOne(d => d.LeaveStatusNavigation).WithMany(p => p.Leaves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_leave_leave_status");
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

        modelBuilder.Entity<SchoolSchedule>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.SchoolSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_school_schedule_employee");

            entity.HasOne(d => d.WeekdayNavigation).WithMany(p => p.SchoolSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_school_schedule_weekday");
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

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.Property(e => e.Concept).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.WorkSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_work_schedule_branch");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.WorkSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_work_schedule_department");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_work_schedule_employee");

            entity.HasOne(d => d.WorkStatusNavigation).WithMany(p => p.WorkSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_work_schedule_work_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

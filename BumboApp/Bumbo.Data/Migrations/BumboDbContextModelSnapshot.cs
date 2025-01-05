﻿// <auto-generated />
using System;
using Bumbo.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bumbo.Data.Migrations
{
    [DbContext(typeof(BumboDbContext))]
    partial class BumboDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bumbo.Data.Models.Availability", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<string>("Weekday")
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("weekday");

                    b.Property<TimeOnly?>("EndTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("end_time");

                    b.Property<TimeOnly?>("StartTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("start_time");

                    b.HasKey("EmployeeId", "Weekday");

                    b.HasIndex("Weekday");

                    b.ToTable("availability");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchId"));

                    b.Property<string>("Addition")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("addition");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("country_code");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int")
                        .HasColumnName("house_number");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(8, 6)")
                        .HasColumnName("latitude");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9, 6)")
                        .HasColumnName("longitude");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("zip_code");

                    b.HasKey("BranchId");

                    b.ToTable("branch");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Department", b =>
                {
                    b.Property<string>("Department1")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("department");

                    b.HasKey("Department1");

                    b.ToTable("department");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Addition")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("addition");

                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<DateOnly>("HiringDate")
                        .HasColumnType("date")
                        .HasColumnName("hiring_date");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int")
                        .HasColumnName("house_number");

                    b.Property<string>("Infix")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("infix");

                    b.Property<string>("LaborContract")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("labor_contract");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("user_id");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("zip_code");

                    b.HasKey("EmployeeId");

                    b.HasIndex("BranchId");

                    b.HasIndex("LaborContract");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("employee");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Forecast", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<string>("Department")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("department");

                    b.Property<int>("ManHours")
                        .HasColumnType("int")
                        .HasColumnName("man_hours");

                    b.HasKey("Date", "BranchId", "Department");

                    b.HasIndex("BranchId");

                    b.HasIndex("Department");

                    b.ToTable("forecast");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Freight", b =>
                {
                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("amount");

                    b.HasKey("BranchId", "Date");

                    b.ToTable("freight");
                });

            modelBuilder.Entity("Bumbo.Data.Models.LaborContract", b =>
                {
                    b.Property<string>("LaborContract1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("labor_contract");

                    b.HasKey("LaborContract1");

                    b.ToTable("labor_contract");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Leave", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<string>("LeaveStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("leave_status");

                    b.HasKey("EmployeeId", "StartDate");

                    b.HasIndex("LeaveStatus");

                    b.ToTable("leave");
                });

            modelBuilder.Entity("Bumbo.Data.Models.LeaveStatus", b =>
                {
                    b.Property<string>("LeaveStatus1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("leave_status");

                    b.HasKey("LeaveStatus1");

                    b.ToTable("leave_status");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Norm", b =>
                {
                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<string>("SupermarketActivity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("supermarket_activity");

                    b.Property<int>("NormPerHour")
                        .HasColumnType("int")
                        .HasColumnName("norm_per_hour");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("unit");

                    b.HasKey("BranchId", "SupermarketActivity");

                    b.HasIndex("SupermarketActivity");

                    b.ToTable("norm");
                });

            modelBuilder.Entity("Bumbo.Data.Models.OpeningHour", b =>
                {
                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<string>("Weekday")
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("weekday");

                    b.Property<TimeOnly>("ClosingTime")
                        .HasColumnType("time")
                        .HasColumnName("closing_time");

                    b.Property<TimeOnly>("OpeningTime")
                        .HasColumnType("time")
                        .HasColumnName("opening_time");

                    b.HasKey("BranchId", "Weekday")
                        .HasName("PK_opening_hours_1");

                    b.HasIndex("Weekday");

                    b.ToTable("opening_hours");
                });

            modelBuilder.Entity("Bumbo.Data.Models.SchoolSchedule", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<string>("Weekday")
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("weekday");

                    b.Property<TimeOnly?>("EndTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("end_time");

                    b.Property<TimeOnly?>("StartTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("start_time");

                    b.HasKey("EmployeeId", "Weekday");

                    b.HasIndex("Weekday");

                    b.ToTable("school_schedule");
                });

            modelBuilder.Entity("Bumbo.Data.Models.StoreTraffic", b =>
                {
                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("date_time");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("amount");

                    b.HasKey("BranchId", "DateTime", "Amount");

                    b.ToTable("store_traffic");
                });

            modelBuilder.Entity("Bumbo.Data.Models.SupermarketActivity", b =>
                {
                    b.Property<string>("SupermarketActivity1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("supermarket_activity");

                    b.HasKey("SupermarketActivity1")
                        .HasName("PK_activity");

                    b.ToTable("supermarket_activity");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Weekday", b =>
                {
                    b.Property<string>("Weekday1")
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("weekday");

                    b.HasKey("Weekday1");

                    b.ToTable("weekday");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkSchedule", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("BranchId")
                        .HasColumnType("int")
                        .HasColumnName("branch_id");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("department");

                    b.Property<TimeOnly>("EndTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("end_time");

                    b.Property<TimeOnly>("StartTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("start_time");

                    b.Property<string>("WorkStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("work_status");

                    b.HasKey("EmployeeId", "Date", "BranchId");

                    b.HasIndex("BranchId");

                    b.HasIndex("Department");

                    b.HasIndex("WorkStatus");

                    b.ToTable("work_schedule");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkShift", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime")
                        .HasColumnName("start_time");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime")
                        .HasColumnName("end_time");

                    b.HasKey("EmployeeId", "StartTime")
                        .HasName("PK_work_shift_1");

                    b.ToTable("work_shift");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkStatus", b =>
                {
                    b.Property<string>("WorkStatus1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("work_status");

                    b.HasKey("WorkStatus1");

                    b.ToTable("work_status");
                });

            modelBuilder.Entity("DepartmentEmployee", b =>
                {
                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Department", "EmployeeId");

                    b.ToTable("DepartmentEmployee");
                });

            modelBuilder.Entity("EmployeeInDepartment", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<string>("Department")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("department");

                    b.HasKey("EmployeeId", "Department");

                    b.HasIndex("Department");

                    b.ToTable("employee_in_department", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator().HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Bumbo.Data.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Availability", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Employee", "Employee")
                        .WithMany("Availabilities")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_availability_employee");

                    b.HasOne("Bumbo.Data.Models.Weekday", "WeekdayNavigation")
                        .WithMany("Availabilities")
                        .HasForeignKey("Weekday")
                        .IsRequired()
                        .HasConstraintName("FK_availability_weekday");

                    b.Navigation("Employee");

                    b.Navigation("WeekdayNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Employee", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("Employees")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_employee_branch");

                    b.HasOne("Bumbo.Data.Models.LaborContract", "LaborContractNavigation")
                        .WithMany("Employees")
                        .HasForeignKey("LaborContract")
                        .IsRequired()
                        .HasConstraintName("FK_employee_labor_contract");

                    b.HasOne("Bumbo.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Employee")
                        .HasForeignKey("Bumbo.Data.Models.Employee", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Branch");

                    b.Navigation("LaborContractNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Forecast", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("Forecasts")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_forecast_branch");

                    b.HasOne("Bumbo.Data.Models.Department", "DepartmentNavigation")
                        .WithMany("Forecasts")
                        .HasForeignKey("Department")
                        .IsRequired()
                        .HasConstraintName("FK_forecast_department");

                    b.Navigation("Branch");

                    b.Navigation("DepartmentNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Freight", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("Freights")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_freight_branch");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Leave", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Employee", "Employee")
                        .WithMany("Leaves")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_leave_employee");

                    b.HasOne("Bumbo.Data.Models.LeaveStatus", "LeaveStatusNavigation")
                        .WithMany("Leaves")
                        .HasForeignKey("LeaveStatus")
                        .IsRequired()
                        .HasConstraintName("FK_leave_leave_status");

                    b.Navigation("Employee");

                    b.Navigation("LeaveStatusNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Norm", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("Norms")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_norm_branch");

                    b.HasOne("Bumbo.Data.Models.SupermarketActivity", "SupermarketActivityNavigation")
                        .WithMany("Norms")
                        .HasForeignKey("SupermarketActivity")
                        .IsRequired()
                        .HasConstraintName("FK_norm_activity");

                    b.Navigation("Branch");

                    b.Navigation("SupermarketActivityNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.OpeningHour", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("OpeningHours")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_opening_hours_branch");

                    b.HasOne("Bumbo.Data.Models.Weekday", "WeekdayNavigation")
                        .WithMany("OpeningHours")
                        .HasForeignKey("Weekday")
                        .IsRequired()
                        .HasConstraintName("FK_opening_hours_weekday");

                    b.Navigation("Branch");

                    b.Navigation("WeekdayNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.SchoolSchedule", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Employee", "Employee")
                        .WithMany("SchoolSchedules")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_school_schedule_employee");

                    b.HasOne("Bumbo.Data.Models.Weekday", "WeekdayNavigation")
                        .WithMany("SchoolSchedules")
                        .HasForeignKey("Weekday")
                        .IsRequired()
                        .HasConstraintName("FK_school_schedule_weekday");

                    b.Navigation("Employee");

                    b.Navigation("WeekdayNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.StoreTraffic", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("StoreTraffics")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_store_traffic_branch");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkSchedule", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Branch", "Branch")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK_work_schedule_branch");

                    b.HasOne("Bumbo.Data.Models.Department", "DepartmentNavigation")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("Department")
                        .IsRequired()
                        .HasConstraintName("FK_work_schedule_department");

                    b.HasOne("Bumbo.Data.Models.Employee", "Employee")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_work_schedule_employee");

                    b.HasOne("Bumbo.Data.Models.WorkStatus", "WorkStatusNavigation")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("WorkStatus")
                        .IsRequired()
                        .HasConstraintName("FK_work_schedule_work_status");

                    b.Navigation("Branch");

                    b.Navigation("DepartmentNavigation");

                    b.Navigation("Employee");

                    b.Navigation("WorkStatusNavigation");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkShift", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Employee", "Employee")
                        .WithMany("WorkShifts")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_work_shift_employee");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeInDepartment", b =>
                {
                    b.HasOne("Bumbo.Data.Models.Department", null)
                        .WithMany()
                        .HasForeignKey("Department")
                        .IsRequired()
                        .HasConstraintName("FK_employee_in_department_department");

                    b.HasOne("Bumbo.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_employee_in_department_employee");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bumbo.Data.Models.Branch", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Forecasts");

                    b.Navigation("Freights");

                    b.Navigation("Norms");

                    b.Navigation("OpeningHours");

                    b.Navigation("StoreTraffics");

                    b.Navigation("WorkSchedules");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Department", b =>
                {
                    b.Navigation("Forecasts");

                    b.Navigation("WorkSchedules");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Employee", b =>
                {
                    b.Navigation("Availabilities");

                    b.Navigation("Leaves");

                    b.Navigation("SchoolSchedules");

                    b.Navigation("WorkSchedules");

                    b.Navigation("WorkShifts");
                });

            modelBuilder.Entity("Bumbo.Data.Models.LaborContract", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Bumbo.Data.Models.LeaveStatus", b =>
                {
                    b.Navigation("Leaves");
                });

            modelBuilder.Entity("Bumbo.Data.Models.SupermarketActivity", b =>
                {
                    b.Navigation("Norms");
                });

            modelBuilder.Entity("Bumbo.Data.Models.Weekday", b =>
                {
                    b.Navigation("Availabilities");

                    b.Navigation("OpeningHours");

                    b.Navigation("SchoolSchedules");
                });

            modelBuilder.Entity("Bumbo.Data.Models.WorkStatus", b =>
                {
                    b.Navigation("WorkSchedules");
                });

            modelBuilder.Entity("Bumbo.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("Employee")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

using HRManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Data
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<EmployeeServiceInfo> EmployeeServiceInfos { get; set; }
        public DbSet<EmployeeAssignment> EmployeeAssignments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EmployeeContact> EmployeeContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MilitaryNumber).IsRequired();
                entity.HasIndex(e => e.MilitaryNumber).IsUnique();
                entity.Property(e => e.ArabicFirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ArabicMiddleName).HasMaxLength(100);
                entity.Property(e => e.ArabicLastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EnglishFirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EnglishMiddleName).HasMaxLength(100);
                entity.Property(e => e.EnglishLastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IdNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.IdNumber).IsUnique();
            });

            // EmployeeProfile configuration
            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SkinColor).HasMaxLength(50);
                entity.Property(e => e.HairColor).HasMaxLength(50);
                entity.Property(e => e.EyeColor).HasMaxLength(50);
                entity.Property(e => e.DisabilityType).HasMaxLength(100);
                entity.Property(e => e.CurrentNationality).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Religion).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PlaceOfBirth).IsRequired().HasMaxLength(200);
                entity.Property(e => e.InsuranceNumber).HasMaxLength(50);
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.Profile)
                    .HasForeignKey<EmployeeProfile>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<EmployeeContact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email);
                entity.Property(e => e.MobileNumber).IsRequired();
                entity.Property(e => e.SecondMobileNumber);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.Contact)
                    .HasForeignKey<EmployeeContact>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // EmployeeServiceInfo configuration
            modelBuilder.Entity<EmployeeServiceInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BaseSalary).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.ServiceInfos)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BelongingUnit)
                    .WithMany()
                    .HasForeignKey(e => e.BelongingUnitId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.JobRole)
                    .WithMany(e => e.EmployeeServiceInfos)
                    .HasForeignKey(e => e.JobRoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // EmployeeAssignment configuration
            modelBuilder.Entity<EmployeeAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssignmentDate).IsRequired();
                entity.Property(e => e.HiringDate).IsRequired();
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.Assignments)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.AssignedUnit)
                    .WithMany(e => e.EmployeeAssignments)
                    .HasForeignKey(e => e.AssignedUnitId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.JobRole)
                    .WithMany(e => e.EmployeeAssignments)
                    .HasForeignKey(e => e.JobRoleId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // OrgUnit configuration
            modelBuilder.Entity<OrgUnit>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(200);
                entity.Property(o => o.Description);
                entity.HasOne(o => o.Parent)
                      .WithMany(o => o.Children)
                      .HasForeignKey(o => o.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Manager)
                      .WithMany()
                      .HasForeignKey(o => o.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // LeaveRequest configuration
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reason).HasMaxLength(500);
                entity.Property(e => e.ManagerComments).HasMaxLength(500);
                // entity.HasOne(e => e.Employee)
                //     .WithMany(e => e.LeaveRequests)
                //     .HasForeignKey(e => e.EmployeeId)
                //     .OnDelete(DeleteBehavior.Cascade);
            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Employee || e.Entity is LeaveRequest ||
                           e.Entity is EmployeeProfile || e.Entity is EmployeeServiceInfo || e.Entity is EmployeeAssignment)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var updatedAtProperty = entity.GetType().GetProperty("UpdatedAt");
                if (updatedAtProperty != null)
                {
                    updatedAtProperty.SetValue(entity, DateTime.UtcNow);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
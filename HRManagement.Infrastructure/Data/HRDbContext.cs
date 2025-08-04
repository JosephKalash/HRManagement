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
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Position).HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            });

            // LeaveRequest configuration
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reason).HasMaxLength(500);
                entity.Property(e => e.ManagerComments).HasMaxLength(500);
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.LeaveRequests)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PerformanceReview configuration
            modelBuilder.Entity<PerformanceReview>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReviewerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Strengths).HasMaxLength(1000);
                entity.Property(e => e.AreasForImprovement).HasMaxLength(1000);
                entity.Property(e => e.Goals).HasMaxLength(1000);
                entity.Property(e => e.Comments).HasMaxLength(1000);
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.PerformanceReviews)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrgUnit configuration
            modelBuilder.Entity<OrgUnit>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(200);
                entity.Property(o => o.Type).IsRequired();
                entity.HasOne(o => o.Parent)
                      .WithMany(o => o.Children)
                      .HasForeignKey(o => o.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Employee || e.Entity is LeaveRequest || e.Entity is PerformanceReview)
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
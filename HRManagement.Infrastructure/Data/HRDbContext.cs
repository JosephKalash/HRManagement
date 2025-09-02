using System.Reflection;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Data
{
    public class HRDbContext(DbContextOptions<HRDbContext> options, ICurrentUserService? currentUser = null) : DbContext(options)
    {
        private readonly ICurrentUserService? _currentUser = currentUser;

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<EmployeeContact> EmployeeContacts { get; set; }
        public DbSet<EmployeeSignature> EmployeeSignatures { get; set; }
        public DbSet<EmployeeServiceInfo> EmployeeServiceInfos { get; set; }
        public DbSet<EmployeeAssignment> EmployeeAssignments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<OrgUnitProfile> OrgUnitProfiles { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<EmployeeRank> EmployeeRanks { get; set; }
        public DbSet<EmploymentDetails> EmploymentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filters to exclude soft-deleted records
            // modelBuilder.Entity<Employee>().Navigation();
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Rank>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeContact>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeSignature>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeServiceInfo>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeAssignment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<OrgUnitProfile>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<OrgUnit>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Nationality>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeRank>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmploymentDetails>().HasQueryFilter(e => !e.IsDeleted);


            //base entity configuration 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<EmploymentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Employee)
                        .WithOne(e => e.EmploymentDetails)
                        .HasForeignKey<EmploymentDetails>(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<EmployeeRank>(entity =>
            {
                entity.HasOne(er => er.Employee)
                       .WithMany(e => e.EmployeeRanks)
                       .HasForeignKey(er => er.EmployeeId)
                       .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(er => er.Rank)
                       .WithMany(r => r.EmployeeRanks)
                       .HasForeignKey(er => er.RankId)
                       .OnDelete(DeleteBehavior.Restrict);

                //todo
                // entity.HasIndex(er => new { er.EmployeeId, er.RankId, er.IsActive }).HasFilter("EmployeeRanks.IsActive = 1");
                entity.HasIndex(er => er.EmployeeId);
                        // .IncludeProperties(er => new { er.RankId, er.IsActive, er.AssignedDate });
                entity.HasIndex(er => new { er.RankId, er.IsActive });
                entity.Property(er => er.Notes).HasMaxLength(500);
            });
            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {

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

                entity.Property(e => e.HairColor).HasMaxLength(50);
                entity.Property(e => e.EyeColor).HasMaxLength(50);
                entity.Property(e => e.DisabilityType).HasMaxLength(100);
                entity.Property(e => e.InsuranceNumber).HasMaxLength(50);

                entity.HasOne(e => e.Nationality)
                           .WithMany(n => n.CurrentEmployees)
                           .HasForeignKey(e => e.NationalityId)
                           .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.PreviousNationality)
                    .WithMany(n => n.PreviousEmployees)
                    .HasForeignKey(e => e.PreviousNationalityId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.Profile)
                    .HasForeignKey<EmployeeProfile>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<EmployeeContact>(entity =>
            {

                entity.Property(e => e.Email);
                entity.Property(e => e.MobileNumber).IsRequired();
                entity.Property(e => e.SecondMobileNumber);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.Contact)
                    .HasForeignKey<EmployeeContact>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // EmployeeSignature configuration
            modelBuilder.Entity<EmployeeSignature>(entity =>
            {

                entity.Property(e => e.SignatureName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OriginalFileName).HasMaxLength(255);
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.Signature)
                    .HasForeignKey<EmployeeSignature>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // EmployeeServiceInfo configuration
            modelBuilder.Entity<EmployeeServiceInfo>(entity =>
            {

                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.ServiceInfos)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BelongingUnit)
                    .WithMany(ou => ou.EmployeeServiceInfos)
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

            modelBuilder.Entity<Rank>(entity =>
            {

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Order).IsRequired();

                entity.HasIndex(e => e.Order).IsUnique();
            });
            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // OrgUnit configuration
            modelBuilder.Entity<OrgUnit>(entity =>
            {
                entity.Property(o => o.OfficialName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Level).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.HasIndex(e => e.Level);
                entity.HasOne(o => o.Parent)
                      .WithMany(o => o.Children)
                      .HasForeignKey(o => o.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Manager)
                      .WithMany()
                      .HasForeignKey(o => o.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.Type);
            });

            modelBuilder.Entity<OrgUnitProfile>(entity =>
            {
                // entity.Property(e => e.Specialization).HasColumnType("nvarchar(max)");
                entity.HasOne(e => e.OrgUnit)
                    .WithOne(e => e.Profile)
                    .HasForeignKey<OrgUnitProfile>(e => e.OrgUnitId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;
            var userId = _currentUser?.UserId;
            var userName = _currentUser?.UserName;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = null;
                    entry.Entity.IsDeleted = false;

                    // Set CreatedBy if available on the entity
                    var createdByProp = entry.Entity.GetType().GetProperty("CreatedBy");
                    if (createdByProp != null)
                    {
                        if (createdByProp.PropertyType == typeof(long?) && userId.HasValue)
                        {
                            createdByProp.SetValue(entry.Entity, userId);
                        }
                        else if (createdByProp.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(userName))
                        {
                            createdByProp.SetValue(entry.Entity, userName);
                        }
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;

                    // Set UpdatedBy if available on the entity
                    var updatedByProp = entry.Entity.GetType().GetProperty("UpdatedBy");
                    if (updatedByProp != null)
                    {
                        if (updatedByProp.PropertyType == typeof(long?) && userId.HasValue)
                        {
                            updatedByProp.SetValue(entry.Entity, userId);
                        }
                        else if (updatedByProp.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(userName))
                        {
                            updatedByProp.SetValue(entry.Entity, userName);
                        }
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Convert hard delete to soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = utcNow;

                    var updatedByProp = entry.Entity.GetType().GetProperty("UpdatedBy");
                    if (updatedByProp != null)
                    {
                        if (updatedByProp.PropertyType == typeof(long?) && userId.HasValue)
                        {
                            updatedByProp.SetValue(entry.Entity, userId);
                        }
                        else if (updatedByProp.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(userName))
                        {
                            updatedByProp.SetValue(entry.Entity, userName);
                        }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
using HRManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagement.Infrastructure.Data;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
       public void Configure(EntityTypeBuilder<T> builder)
       {
              builder.HasKey(e => e.Id);
              builder.Property(e => e.Id)
                     .ValueGeneratedOnAdd();

              builder.HasIndex(e => e.Guid)
                     .IsUnique();

              builder.Property(e => e.CreatedAt)
                     .HasDefaultValueSql("GETUTCDATE()");
       }
}


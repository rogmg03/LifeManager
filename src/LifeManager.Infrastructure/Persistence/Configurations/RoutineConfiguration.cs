using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class RoutineConfiguration : IEntityTypeConfiguration<Routine>
{
    public void Configure(EntityTypeBuilder<Routine> builder)
    {
        builder.ToTable("Routines");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(256);
        builder.Property(r => r.Description).HasColumnType("text");
        builder.Property(r => r.EstimatedDurationMinutes);
        builder.Property(r => r.Category).HasMaxLength(64);
        builder.Property(r => r.IsArchived).IsRequired().HasDefaultValue(false);
        builder.Property(r => r.SortOrder).IsRequired();

        builder.HasIndex(r => new { r.UserId, r.IsArchived });
        builder.HasIndex(r => new { r.UserId, r.SortOrder });

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

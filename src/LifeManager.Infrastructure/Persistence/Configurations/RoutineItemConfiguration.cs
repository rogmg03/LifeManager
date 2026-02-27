using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class RoutineItemConfiguration : IEntityTypeConfiguration<RoutineItem>
{
    public void Configure(EntityTypeBuilder<RoutineItem> builder)
    {
        builder.ToTable("RoutineItems");
        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.ExerciseName).IsRequired().HasMaxLength(256);
        builder.Property(ri => ri.Description).HasColumnType("text");
        builder.Property(ri => ri.TargetSets).IsRequired();
        builder.Property(ri => ri.TargetReps).IsRequired();
        builder.Property(ri => ri.TargetWeight).HasPrecision(18, 2);
        builder.Property(ri => ri.RestSeconds).IsRequired().HasDefaultValue(60);
        builder.Property(ri => ri.SortOrder).IsRequired();

        builder.HasIndex(ri => new { ri.RoutineId, ri.SortOrder });

        builder.HasOne(ri => ri.Routine)
            .WithMany(r => r.Items)
            .HasForeignKey(ri => ri.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

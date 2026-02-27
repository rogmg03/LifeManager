using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class WorkoutSetConfiguration : IEntityTypeConfiguration<WorkoutSet>
{
    public void Configure(EntityTypeBuilder<WorkoutSet> builder)
    {
        builder.ToTable("WorkoutSets");
        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.ExerciseName).IsRequired().HasMaxLength(256);
        builder.Property(ws => ws.SetNumber).IsRequired();
        builder.Property(ws => ws.TargetReps).IsRequired();
        builder.Property(ws => ws.TargetWeight).HasPrecision(18, 2);
        builder.Property(ws => ws.ActualReps);
        builder.Property(ws => ws.ActualWeight).HasPrecision(18, 2);
        builder.Property(ws => ws.IsCompleted).IsRequired().HasDefaultValue(false);
        builder.Property(ws => ws.CompletedAt);

        builder.HasIndex(ws => new { ws.SessionId, ws.RoutineItemId, ws.SetNumber });

        builder.HasOne(ws => ws.Session)
            .WithMany(s => s.Sets)
            .HasForeignKey(ws => ws.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ws => ws.RoutineItem)
            .WithMany()
            .HasForeignKey(ws => ws.RoutineItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

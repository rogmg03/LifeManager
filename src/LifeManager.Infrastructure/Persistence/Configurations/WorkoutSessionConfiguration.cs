using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions");
        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.RoutineName).IsRequired().HasMaxLength(256);
        builder.Property(ws => ws.StartedAt).IsRequired();
        builder.Property(ws => ws.CompletedAt);
        builder.Property(ws => ws.DurationSeconds);
        builder.Property(ws => ws.TotalSets).IsRequired();
        builder.Property(ws => ws.CompletedSets).IsRequired().HasDefaultValue(0);
        builder.Property(ws => ws.CompletionRate).IsRequired().HasPrecision(5, 2);
        builder.Property(ws => ws.Notes).HasColumnType("text");

        builder.HasIndex(ws => new { ws.UserId, ws.StartedAt });
        builder.HasIndex(ws => ws.RoutineId);

        builder.HasOne(ws => ws.User)
            .WithMany()
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ws => ws.Routine)
            .WithMany(r => r.Sessions)
            .HasForeignKey(ws => ws.RoutineId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

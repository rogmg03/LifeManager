using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class WorkoutLogConfiguration : IEntityTypeConfiguration<WorkoutLog>
{
    public void Configure(EntityTypeBuilder<WorkoutLog> builder)
    {
        builder.ToTable("WorkoutLogs");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.LoggedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(w => w.Notes).HasColumnType("text");

        builder.HasIndex(w => new { w.ProjectId, w.LoggedAt });

        builder.HasOne(w => w.Project)
            .WithMany()
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(w => w.Routine)
            .WithMany()
            .HasForeignKey(w => w.RoutineId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ExerciseGoalConfiguration : IEntityTypeConfiguration<ExerciseGoal>
{
    public void Configure(EntityTypeBuilder<ExerciseGoal> builder)
    {
        builder.ToTable("ExerciseGoals");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.MetricName).IsRequired().HasMaxLength(256);
        builder.Property(g => g.TargetValue).HasPrecision(18, 2).IsRequired();
        builder.Property(g => g.Unit).IsRequired().HasMaxLength(64);

        builder.HasOne(g => g.Project)
            .WithMany()
            .HasForeignKey(g => g.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

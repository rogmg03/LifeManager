using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class DailyGoalConfiguration : IEntityTypeConfiguration<DailyGoal>
{
    public void Configure(EntityTypeBuilder<DailyGoal> builder)
    {
        builder.ToTable("DailyGoals");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Category)
            .HasConversion<string>()
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(g => g.GoalMinutes)
            .IsRequired();

        // Unique constraint: one goal per user per category
        builder.HasIndex(g => new { g.UserId, g.Category }).IsUnique();

        builder.HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

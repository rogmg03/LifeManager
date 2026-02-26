using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).IsRequired().HasMaxLength(512);
        builder.Property(t => t.Description).HasColumnType("text");
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(16);
        builder.Property(t => t.Priority).HasConversion<string>().HasMaxLength(16);

        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.PhaseId);
        builder.HasIndex(t => t.Status);

        builder.HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Phase)
            .WithMany()
            .HasForeignKey(t => t.PhaseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Subtasks)
            .WithOne(s => s.Task)
            .HasForeignKey(s => s.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.RecurrenceRule)
            .WithOne(r => r.Task)
            .HasForeignKey<RecurrenceRule>(r => r.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

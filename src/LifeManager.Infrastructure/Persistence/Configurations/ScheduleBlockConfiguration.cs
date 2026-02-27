using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ScheduleBlockConfiguration : IEntityTypeConfiguration<ScheduleBlock>
{
    public void Configure(EntityTypeBuilder<ScheduleBlock> builder)
    {
        builder.ToTable("ScheduleBlocks");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(256);
        builder.Property(b => b.Notes).HasColumnType("text");
        builder.Property(b => b.BlockType).HasConversion<string>().HasMaxLength(16);
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(16);

        // UTC timestamps
        builder.Property(b => b.StartTime).HasColumnType("timestamptz");
        builder.Property(b => b.EndTime).HasColumnType("timestamptz");

        builder.HasIndex(b => new { b.UserId, b.StartTime });

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Optional Project link
        builder.HasOne(b => b.Project)
            .WithMany()
            .HasForeignKey(b => b.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        // Optional Task link
        builder.HasOne(b => b.Task)
            .WithMany()
            .HasForeignKey(b => b.TaskId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

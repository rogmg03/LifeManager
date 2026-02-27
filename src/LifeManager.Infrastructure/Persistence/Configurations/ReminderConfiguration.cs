using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ToTable("Reminders");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title).IsRequired().HasMaxLength(256);
        builder.Property(r => r.Notes).HasColumnType("text");
        builder.Property(r => r.ReminderType).HasConversion<string>().HasMaxLength(32);
        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(16);
        builder.Property(r => r.RemindAt).IsRequired();

        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => new { r.UserId, r.Status });

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Task)
            .WithMany()
            .HasForeignKey(r => r.TaskId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.ScheduleBlock)
            .WithMany()
            .HasForeignKey(r => r.ScheduleBlockId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

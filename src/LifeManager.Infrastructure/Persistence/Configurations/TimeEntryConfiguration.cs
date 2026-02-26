using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.ToTable("TimeEntries");
        builder.HasKey(te => te.Id);

        builder.Property(te => te.Notes).HasColumnType("text");
        builder.Property(te => te.IsManual).HasDefaultValue(false);

        builder.HasIndex(te => te.UserId);
        builder.HasIndex(te => te.TaskId);
        builder.HasIndex(te => te.StartedAt);
        // Partial index: only one running timer per user
        builder.HasIndex(te => new { te.UserId, te.EndedAt })
            .HasFilter("\"EndedAt\" IS NULL")
            .IsUnique();

        builder.HasOne(te => te.Task)
            .WithMany()
            .HasForeignKey(te => te.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(te => te.User)
            .WithMany()
            .HasForeignKey(te => te.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cycle 10 — nullable FK to FreeTimeTransaction (set by event handler after earn)
        builder.HasOne(te => te.EarnedTransaction)
            .WithMany()
            .HasForeignKey(te => te.EarnedTransactionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

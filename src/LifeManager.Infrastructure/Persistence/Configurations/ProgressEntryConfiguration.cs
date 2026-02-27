using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ProgressEntryConfiguration : IEntityTypeConfiguration<ProgressEntry>
{
    public void Configure(EntityTypeBuilder<ProgressEntry> builder)
    {
        builder.ToTable("ProgressEntries");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RecordedAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(e => e.Value).HasPrecision(18, 2).IsRequired();
        builder.Property(e => e.Notes).HasColumnType("text");

        builder.HasIndex(e => new { e.GoalId, e.RecordedAt });

        builder.HasOne(e => e.Goal)
            .WithMany()
            .HasForeignKey(e => e.GoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

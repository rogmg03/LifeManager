using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class FreeTimeTransactionConfiguration : IEntityTypeConfiguration<FreeTimeTransaction>
{
    public void Configure(EntityTypeBuilder<FreeTimeTransaction> builder)
    {
        builder.ToTable("FreeTimeTransactions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(16);
        builder.Property(t => t.Notes).HasColumnType("text");
        builder.Property(t => t.ScheduleBlockId); // plain column — FK wired in Cycle 11

        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.CreatedAt);

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // TimeEntry → FreeTimeTransaction: SetNull when TimeEntry is deleted
        builder.HasOne(t => t.TimeEntry)
            .WithMany()
            .HasForeignKey(t => t.TimeEntryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

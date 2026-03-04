using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class GoogleCalendarSyncConfiguration : IEntityTypeConfiguration<GoogleCalendarSync>
{
    public void Configure(EntityTypeBuilder<GoogleCalendarSync> builder)
    {
        builder.ToTable("GoogleCalendarSyncs");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.CalendarId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(g => g.LastSyncedAt)
            .IsRequired(false);

        builder.Property(g => g.IsEnabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(g => g.UserId);

        builder.HasIndex(g => new { g.UserId, g.CalendarId })
            .IsUnique();

        builder.HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

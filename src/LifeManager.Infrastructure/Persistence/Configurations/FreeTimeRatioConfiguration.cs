using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class FreeTimeRatioConfiguration : IEntityTypeConfiguration<FreeTimeRatio>
{
    public void Configure(EntityTypeBuilder<FreeTimeRatio> builder)
    {
        builder.ToTable("FreeTimeRatios");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.WorkMinutesPerFreeMinute)
            .HasPrecision(6, 2)
            .HasDefaultValue(1.0m);

        builder.HasIndex(r => r.UserId).IsUnique(); // 1:1 with User

        builder.HasOne(r => r.User)
            .WithOne()
            .HasForeignKey<FreeTimeRatio>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

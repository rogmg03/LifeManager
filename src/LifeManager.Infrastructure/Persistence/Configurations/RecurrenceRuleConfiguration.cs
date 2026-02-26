using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class RecurrenceRuleConfiguration : IEntityTypeConfiguration<RecurrenceRule>
{
    public void Configure(EntityTypeBuilder<RecurrenceRule> builder)
    {
        builder.ToTable("RecurrenceRules");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Pattern).HasConversion<string>().HasMaxLength(16);
        builder.Property(r => r.DaysOfWeek).HasMaxLength(64);
        builder.Property(r => r.IsActive).IsRequired();
        builder.Property(r => r.NextDueDate).IsRequired();

        builder.HasIndex(r => r.TaskId).IsUnique();
    }
}

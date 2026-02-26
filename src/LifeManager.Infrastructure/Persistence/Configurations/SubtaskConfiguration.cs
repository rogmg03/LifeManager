using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class SubtaskConfiguration : IEntityTypeConfiguration<Subtask>
{
    public void Configure(EntityTypeBuilder<Subtask> builder)
    {
        builder.ToTable("Subtasks");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title).IsRequired().HasMaxLength(512);
        builder.Property(s => s.IsCompleted).IsRequired();
        builder.Property(s => s.SortOrder).IsRequired();

        builder.HasIndex(s => s.TaskId);
    }
}

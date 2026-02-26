using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(512);
        builder.Property(a => a.StoragePath).IsRequired().HasMaxLength(1024);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(256);
        builder.Property(a => a.FileSizeBytes).IsRequired();

        builder.HasIndex(a => a.DocumentId);
        builder.HasIndex(a => a.TaskId);

        // DocumentId FK is configured from DocumentConfiguration (HasMany side)
        // TaskId FK — optional relationship to ProjectTask
        builder.HasOne(a => a.Task)
            .WithMany()
            .HasForeignKey(a => a.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

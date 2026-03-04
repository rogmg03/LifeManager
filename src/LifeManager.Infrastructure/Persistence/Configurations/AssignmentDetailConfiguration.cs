using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class AssignmentDetailConfiguration : IEntityTypeConfiguration<AssignmentDetail>
{
    public void Configure(EntityTypeBuilder<AssignmentDetail> builder)
    {
        builder.ToTable("AssignmentDetails");
        builder.HasKey(d => d.TaskId);

        builder.Property(d => d.AssignmentType)
            .HasConversion<string>()
            .HasMaxLength(64);

        builder.Property(d => d.GradeLetter).HasMaxLength(8);
        builder.Property(d => d.SubmissionLink).HasMaxLength(2048);
        builder.Property(d => d.Grade).HasPrecision(5, 2);
        builder.Property(d => d.Weight).HasPrecision(5, 2);

        builder.HasOne(d => d.Task)
            .WithOne()
            .HasForeignKey<AssignmentDetail>(d => d.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

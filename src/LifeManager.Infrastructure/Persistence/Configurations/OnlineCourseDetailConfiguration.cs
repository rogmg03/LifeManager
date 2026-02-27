using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class OnlineCourseDetailConfiguration : IEntityTypeConfiguration<OnlineCourseDetail>
{
    public void Configure(EntityTypeBuilder<OnlineCourseDetail> builder)
    {
        builder.ToTable("OnlineCourseDetails");
        builder.HasKey(d => d.ProjectId);

        builder.Property(d => d.Platform).HasMaxLength(128);
        builder.Property(d => d.CourseUrl).HasMaxLength(2048);
        builder.Property(d => d.InstructorName).HasMaxLength(256);
        builder.Property(d => d.CertificateUrl).HasMaxLength(2048);

        builder.HasOne(d => d.Project)
            .WithOne()
            .HasForeignKey<OnlineCourseDetail>(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

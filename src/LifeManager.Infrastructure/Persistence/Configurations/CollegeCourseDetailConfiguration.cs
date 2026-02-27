using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class CollegeCourseDetailConfiguration : IEntityTypeConfiguration<CollegeCourseDetail>
{
    public void Configure(EntityTypeBuilder<CollegeCourseDetail> builder)
    {
        builder.ToTable("CollegeCourseDetails");
        builder.HasKey(d => d.ProjectId);

        builder.Property(d => d.InstitutionName).HasMaxLength(256);
        builder.Property(d => d.CourseName).HasMaxLength(256);
        builder.Property(d => d.CourseCode).HasMaxLength(64);
        builder.Property(d => d.Semester).HasMaxLength(64);
        builder.Property(d => d.Professor).HasMaxLength(256);
        builder.Property(d => d.Room).HasMaxLength(128);
        builder.Property(d => d.Schedule).HasColumnType("text");
        builder.Property(d => d.Credits).HasPrecision(5, 2);
        builder.Property(d => d.CurrentGrade).HasPrecision(5, 2);
        builder.Property(d => d.TargetGrade).HasPrecision(5, 2);

        builder.HasOne(d => d.Project)
            .WithOne()
            .HasForeignKey<CollegeCourseDetail>(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

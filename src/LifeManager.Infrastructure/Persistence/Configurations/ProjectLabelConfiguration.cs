using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class ProjectLabelConfiguration : IEntityTypeConfiguration<ProjectLabel>
{
    public void Configure(EntityTypeBuilder<ProjectLabel> builder)
    {
        builder.ToTable("ProjectLabels");
        builder.HasKey(pl => new { pl.ProjectId, pl.LabelId });

        builder.HasOne(pl => pl.Project)
            .WithMany()
            .HasForeignKey(pl => pl.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pl => pl.Label)
            .WithMany()
            .HasForeignKey(pl => pl.LabelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

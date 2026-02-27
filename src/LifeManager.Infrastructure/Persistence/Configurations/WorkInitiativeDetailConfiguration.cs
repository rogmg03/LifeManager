using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeManager.Infrastructure.Persistence.Configurations;

public class WorkInitiativeDetailConfiguration : IEntityTypeConfiguration<WorkInitiativeDetail>
{
    public void Configure(EntityTypeBuilder<WorkInitiativeDetail> builder)
    {
        builder.ToTable("WorkInitiativeDetails");
        builder.HasKey(d => d.ProjectId);

        builder.Property(d => d.ClientName).HasMaxLength(256);
        builder.Property(d => d.BillingType).HasMaxLength(64);
        builder.Property(d => d.ContractValue).HasPrecision(18, 2);
        builder.Property(d => d.HourlyRate).HasPrecision(18, 2);
        builder.Property(d => d.EstimatedHours).HasPrecision(18, 2);
        builder.Property(d => d.LoggedHours).HasPrecision(18, 2);

        builder.HasOne(d => d.Project)
            .WithOne()
            .HasForeignKey<WorkInitiativeDetail>(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

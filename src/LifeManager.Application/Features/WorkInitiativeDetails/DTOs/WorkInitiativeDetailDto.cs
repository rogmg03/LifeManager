using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.WorkInitiativeDetails.DTOs;

public record WorkInitiativeDetailDto(
    Guid ProjectId,
    string? ClientName,
    decimal? ContractValue,
    string? BillingType,
    decimal? HourlyRate,
    decimal? EstimatedHours,
    decimal? LoggedHours,
    bool IsInternal)
{
    public static WorkInitiativeDetailDto FromEntity(WorkInitiativeDetail d) => new(
        d.ProjectId,
        d.ClientName,
        d.ContractValue,
        d.BillingType,
        d.HourlyRate,
        d.EstimatedHours,
        d.LoggedHours,
        d.IsInternal);
}

namespace LifeManager.Application.Features.WorkInitiativeDetails.DTOs;

public record CreateWorkInitiativeDetailRequest(
    string? ClientName,
    decimal? ContractValue,
    string? BillingType,
    decimal? HourlyRate,
    decimal? EstimatedHours,
    decimal? LoggedHours,
    bool IsInternal = false);

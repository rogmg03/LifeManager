namespace LifeManager.Application.Features.TimeTracking.DTOs;

public record CreateManualTimeEntryRequest(
    DateTime StartedAt,
    DateTime EndedAt,
    string? Notes);

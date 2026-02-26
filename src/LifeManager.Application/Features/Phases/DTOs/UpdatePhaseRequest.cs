using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Phases.DTOs;

public record UpdatePhaseRequest(
    string Name,
    string? Description,
    int SortOrder,
    Priority? Priority,
    PhaseStatus Status,
    DateOnly? StartDate,
    DateOnly? EndDate);

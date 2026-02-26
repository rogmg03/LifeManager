using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Phases.DTOs;

public record CreatePhaseRequest(
    string Name,
    string? Description,
    int SortOrder,
    Priority? Priority,
    DateOnly? StartDate,
    DateOnly? EndDate);

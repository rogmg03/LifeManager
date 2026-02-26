using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Phases.DTOs;

public record PhaseDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Description,
    int SortOrder,
    string? Priority,
    string Status,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static PhaseDto FromEntity(Phase p) => new(
        p.Id, p.ProjectId,
        p.Name, p.Description,
        p.SortOrder,
        p.Priority?.ToString(),
        p.Status.ToString(),
        p.StartDate, p.EndDate,
        p.CreatedAt, p.UpdatedAt);
}

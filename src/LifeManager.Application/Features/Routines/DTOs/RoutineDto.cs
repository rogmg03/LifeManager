using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Routines.DTOs;

public record RoutineDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Description,
    int? DayOfWeek,
    int SortOrder,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static RoutineDto FromEntity(Routine r) => new(
        r.Id, r.ProjectId, r.Name, r.Description,
        r.DayOfWeek, r.SortOrder, r.CreatedAt, r.UpdatedAt);
}

using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.CourseModules.DTOs;

public record CourseModuleDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Description,
    int SortOrder,
    bool IsCompleted,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static CourseModuleDto FromEntity(CourseModule m) => new(
        m.Id,
        m.ProjectId,
        m.Name,
        m.Description,
        m.SortOrder,
        m.IsCompleted,
        m.Notes,
        m.CreatedAt,
        m.UpdatedAt);
}

using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Tasks.DTOs;

public record TaskDto(
    Guid Id,
    Guid ProjectId,
    Guid? PhaseId,
    string Title,
    string? Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int SortOrder,
    DateTime? CompletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static TaskDto FromEntity(ProjectTask t) => new(
        t.Id, t.ProjectId, t.PhaseId,
        t.Title, t.Description,
        t.Status.ToString(), t.Priority.ToString(),
        t.DueDate, t.EstimatedMinutes, t.SortOrder,
        t.CompletedAt, t.CreatedAt, t.UpdatedAt);
}

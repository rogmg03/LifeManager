using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Tasks.DTOs;

public record CreateTaskRequest(
    string Title,
    string? Description,
    TaskPriority Priority,
    Guid? PhaseId,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int SortOrder = 0);

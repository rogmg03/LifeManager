using LifeManager.Domain.Enums;
using TaskStatus = LifeManager.Domain.Enums.TaskStatus;
namespace LifeManager.Application.Features.Tasks.DTOs;

public record UpdateTaskRequest(
    string Title,
    string? Description,
    TaskStatus Status,
    TaskPriority Priority,
    Guid? PhaseId,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int SortOrder);

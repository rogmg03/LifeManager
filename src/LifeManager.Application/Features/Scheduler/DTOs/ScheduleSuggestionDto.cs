namespace LifeManager.Application.Features.Scheduler.DTOs;

public record ScheduleSuggestionDto(
    Guid TaskId,
    string Title,
    Guid ProjectId,
    string ProjectName,
    DateTime? DueDate,
    string Priority,
    int? EstimatedMinutes);

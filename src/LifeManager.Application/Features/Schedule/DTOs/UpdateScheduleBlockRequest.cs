namespace LifeManager.Application.Features.Schedule.DTOs;

public record UpdateScheduleBlockRequest(
    string Title,
    string? Notes,
    Guid? ProjectId,
    Guid? TaskId);

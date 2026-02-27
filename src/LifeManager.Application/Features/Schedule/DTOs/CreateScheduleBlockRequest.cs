using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.Schedule.DTOs;

public record CreateScheduleBlockRequest(
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    BlockType BlockType,
    string? Notes,
    Guid? ProjectId,
    Guid? TaskId);

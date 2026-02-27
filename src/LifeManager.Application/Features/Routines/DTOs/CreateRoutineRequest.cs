namespace LifeManager.Application.Features.Routines.DTOs;

public record CreateRoutineRequest(
    string Name,
    string? Description,
    int? EstimatedDurationMinutes,
    string? Category,
    int SortOrder = 0);

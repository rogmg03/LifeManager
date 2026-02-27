namespace LifeManager.Application.Features.Routines.DTOs;

public record UpdateRoutineRequest(
    string Name,
    string? Description,
    int? EstimatedDurationMinutes,
    string? Category,
    int? SortOrder);

namespace LifeManager.Application.Features.Routines.DTOs;

public record CreateRoutineRequest(
    string Name,
    string? Description,
    int? DayOfWeek,
    int SortOrder);

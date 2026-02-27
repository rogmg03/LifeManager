namespace LifeManager.Application.Features.Routines.DTOs;

public record UpdateRoutineRequest(
    string Name,
    string? Description,
    int? DayOfWeek,
    int SortOrder);

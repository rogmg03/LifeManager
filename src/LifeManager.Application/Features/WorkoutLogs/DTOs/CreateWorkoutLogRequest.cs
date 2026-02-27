namespace LifeManager.Application.Features.WorkoutLogs.DTOs;

public record CreateWorkoutLogRequest(
    Guid? RoutineId,
    DateTimeOffset LoggedAt,
    string? Notes,
    int? DurationMinutes);

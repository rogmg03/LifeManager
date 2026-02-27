namespace LifeManager.Application.Features.Workouts.DTOs;

public record WorkoutStatsDto(
    int TotalSessions,
    decimal AvgCompletionRate,
    int CurrentStreak,
    int SessionsThisWeek,
    int SessionsThisMonth);

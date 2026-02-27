namespace LifeManager.Application.Features.Workouts.DTOs;

public record LogSetRequest(
    int? ActualReps,
    decimal? ActualWeight,
    bool IsCompleted);

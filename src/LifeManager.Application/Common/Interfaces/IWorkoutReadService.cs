using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Application.Features.Workouts.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface IWorkoutReadService
{
    Task<List<RoutineDto>> GetRoutinesAsync(Guid userId, bool includeArchived, CancellationToken ct = default);
    Task<WorkoutStatsDto> GetWorkoutStatsAsync(Guid userId, CancellationToken ct = default);
    Task<ExerciseProgressDto> GetExerciseProgressAsync(Guid userId, string exerciseName, CancellationToken ct = default);
}

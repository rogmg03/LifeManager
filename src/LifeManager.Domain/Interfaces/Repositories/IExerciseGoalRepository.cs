using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IExerciseGoalRepository
{
    Task<List<ExerciseGoal>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<ExerciseGoal?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(ExerciseGoal goal, CancellationToken ct = default);
    void Update(ExerciseGoal goal);
    void Delete(ExerciseGoal goal);
}

using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IWorkoutLogRepository
{
    Task<List<WorkoutLog>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<WorkoutLog?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(WorkoutLog log, CancellationToken ct = default);
    void Delete(WorkoutLog log);
}

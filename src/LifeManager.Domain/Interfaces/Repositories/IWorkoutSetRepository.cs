using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IWorkoutSetRepository
{
    Task<List<WorkoutSet>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
    Task<WorkoutSet?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(WorkoutSet set, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<WorkoutSet> sets, CancellationToken ct = default);
    void Update(WorkoutSet set);
    void Delete(WorkoutSet set);
}

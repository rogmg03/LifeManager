using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IWorkoutSessionRepository
{
    Task<List<WorkoutSession>> GetByUserIdAsync(Guid userId, Guid? routineId = null, DateTimeOffset? from = null, DateTimeOffset? to = null, int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<WorkoutSession?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<WorkoutSession?> GetByIdWithSetsAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(WorkoutSession session, CancellationToken ct = default);
    void Update(WorkoutSession session);
    void Delete(WorkoutSession session);
}

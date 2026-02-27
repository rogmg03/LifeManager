using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IRoutineItemRepository
{
    Task<List<RoutineItem>> GetByRoutineIdAsync(Guid routineId, CancellationToken ct = default);
    Task<RoutineItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(RoutineItem item, CancellationToken ct = default);
    void Update(RoutineItem item);
    void Delete(RoutineItem item);
}

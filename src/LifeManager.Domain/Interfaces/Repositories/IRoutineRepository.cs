using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IRoutineRepository
{
    Task<List<Routine>> GetByUserIdAsync(Guid userId, bool includeArchived = false, CancellationToken ct = default);
    Task<Routine?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Routine?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Routine routine, CancellationToken ct = default);
    void Update(Routine routine);
    void Delete(Routine routine);
}

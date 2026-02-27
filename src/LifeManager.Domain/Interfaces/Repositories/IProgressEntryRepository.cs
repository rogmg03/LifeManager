using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IProgressEntryRepository
{
    Task<List<ProgressEntry>> GetByGoalIdAsync(Guid goalId, CancellationToken ct = default);
    Task<ProgressEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(ProgressEntry entry, CancellationToken ct = default);
    void Delete(ProgressEntry entry);
}

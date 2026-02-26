using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IRecurrenceRuleRepository
{
    Task<RecurrenceRule?> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task<RecurrenceRule?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(RecurrenceRule rule, CancellationToken ct = default);
    void Update(RecurrenceRule rule);
    void Delete(RecurrenceRule rule);
}

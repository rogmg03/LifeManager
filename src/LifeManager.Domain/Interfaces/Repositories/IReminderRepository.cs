using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IReminderRepository
{
    Task<List<Reminder>> GetByUserIdAsync(Guid userId, bool pendingOnly, CancellationToken ct = default);
    Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Reminder reminder, CancellationToken ct = default);
    void Update(Reminder reminder);
    void Delete(Reminder reminder);
}

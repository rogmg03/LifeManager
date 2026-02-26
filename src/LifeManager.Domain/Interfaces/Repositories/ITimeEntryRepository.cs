using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TimeEntry?> GetActiveTimerByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<TimeEntry?> GetRunningByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task<List<TimeEntry>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task<List<TimeEntry>> GetByDateRangeAsync(Guid userId, DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(TimeEntry entry, CancellationToken ct = default);
    void Update(TimeEntry entry);
    void Delete(TimeEntry entry);
}

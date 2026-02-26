using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IActivityEntryRepository
{
    Task<(List<ActivityEntry> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        Guid? projectId,
        ActivityType? type,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<List<ActivityEntry>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken ct = default);

    Task AddAsync(ActivityEntry entry, CancellationToken ct = default);
}

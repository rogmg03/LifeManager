using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IScheduleBlockRepository
{
    Task<List<ScheduleBlock>> GetByDateRangeAsync(
        Guid userId, DateTime from, DateTime to, CancellationToken ct = default);

    Task<ScheduleBlock?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(ScheduleBlock block, CancellationToken ct = default);
    void Update(ScheduleBlock block);
    void Delete(ScheduleBlock block);
}

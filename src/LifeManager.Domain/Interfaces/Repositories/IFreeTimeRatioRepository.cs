using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IFreeTimeRatioRepository
{
    Task<FreeTimeRatio?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(FreeTimeRatio ratio, CancellationToken ct = default);
    void Update(FreeTimeRatio ratio);
}

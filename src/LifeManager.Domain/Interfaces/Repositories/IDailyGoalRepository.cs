using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IDailyGoalRepository
{
    Task<IReadOnlyList<DailyGoal>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<DailyGoal?> GetByUserAndCategoryAsync(Guid userId, DailyGoalCategory category, CancellationToken ct = default);
    Task AddAsync(DailyGoal goal, CancellationToken ct = default);
    void Update(DailyGoal goal);
}

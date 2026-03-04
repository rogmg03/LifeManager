using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class DailyGoalRepository : IDailyGoalRepository
{
    private readonly AppDbContext _context;
    public DailyGoalRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<DailyGoal>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.DailyGoals
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.Category)
            .ToListAsync(ct);

    public async Task<DailyGoal?> GetByUserAndCategoryAsync(Guid userId, DailyGoalCategory category, CancellationToken ct = default)
        => await _context.DailyGoals
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Category == category, ct);

    public async Task AddAsync(DailyGoal goal, CancellationToken ct = default)
        => await _context.DailyGoals.AddAsync(goal, ct);

    public void Update(DailyGoal goal)
        => _context.DailyGoals.Update(goal);
}

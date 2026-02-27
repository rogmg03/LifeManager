using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ExerciseGoalRepository : IExerciseGoalRepository
{
    private readonly AppDbContext _context;
    public ExerciseGoalRepository(AppDbContext context) => _context = context;

    public async Task<List<ExerciseGoal>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.ExerciseGoals
            .Where(g => g.ProjectId == projectId)
            .OrderBy(g => g.CreatedAt)
            .ToListAsync(ct);

    public async Task<ExerciseGoal?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.ExerciseGoals.FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task AddAsync(ExerciseGoal goal, CancellationToken ct = default)
        => await _context.ExerciseGoals.AddAsync(goal, ct);

    public void Update(ExerciseGoal goal) => _context.ExerciseGoals.Update(goal);

    public void Delete(ExerciseGoal goal) => _context.ExerciseGoals.Remove(goal);
}

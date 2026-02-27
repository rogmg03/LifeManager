using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class WorkoutSetRepository : IWorkoutSetRepository
{
    private readonly AppDbContext _context;
    public WorkoutSetRepository(AppDbContext context) => _context = context;

    public async Task<List<WorkoutSet>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
        => await _context.WorkoutSets
            .Where(ws => ws.SessionId == sessionId)
            .OrderBy(ws => ws.SetNumber)
            .ToListAsync(ct);

    public async Task<WorkoutSet?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.WorkoutSets.FirstOrDefaultAsync(ws => ws.Id == id, ct);

    public async Task AddAsync(WorkoutSet set, CancellationToken ct = default)
        => await _context.WorkoutSets.AddAsync(set, ct);

    public async Task AddRangeAsync(IEnumerable<WorkoutSet> sets, CancellationToken ct = default)
        => await _context.WorkoutSets.AddRangeAsync(sets, ct);

    public void Update(WorkoutSet set) => _context.WorkoutSets.Update(set);

    public void Delete(WorkoutSet set) => _context.WorkoutSets.Remove(set);
}

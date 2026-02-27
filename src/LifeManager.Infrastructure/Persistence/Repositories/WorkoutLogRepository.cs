using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class WorkoutLogRepository : IWorkoutLogRepository
{
    private readonly AppDbContext _context;
    public WorkoutLogRepository(AppDbContext context) => _context = context;

    public async Task<List<WorkoutLog>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.WorkoutLogs
            .Where(w => w.ProjectId == projectId)
            .OrderByDescending(w => w.LoggedAt)
            .ToListAsync(ct);

    public async Task<WorkoutLog?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.WorkoutLogs.FirstOrDefaultAsync(w => w.Id == id, ct);

    public async Task AddAsync(WorkoutLog log, CancellationToken ct = default)
        => await _context.WorkoutLogs.AddAsync(log, ct);

    public void Delete(WorkoutLog log) => _context.WorkoutLogs.Remove(log);
}

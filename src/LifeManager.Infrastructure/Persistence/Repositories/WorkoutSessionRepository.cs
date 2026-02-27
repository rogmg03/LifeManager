using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class WorkoutSessionRepository : IWorkoutSessionRepository
{
    private readonly AppDbContext _context;
    public WorkoutSessionRepository(AppDbContext context) => _context = context;

    public async Task<List<WorkoutSession>> GetByUserIdAsync(
        Guid userId,
        Guid? routineId = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = _context.WorkoutSessions.Where(ws => ws.UserId == userId);
        if (routineId.HasValue)
            query = query.Where(ws => ws.RoutineId == routineId.Value);
        if (from.HasValue)
            query = query.Where(ws => ws.StartedAt >= from.Value);
        if (to.HasValue)
            query = query.Where(ws => ws.StartedAt <= to.Value);
        return await query
            .OrderByDescending(ws => ws.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<WorkoutSession?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.WorkoutSessions.FirstOrDefaultAsync(ws => ws.Id == id, ct);

    public async Task<WorkoutSession?> GetByIdWithSetsAsync(Guid id, CancellationToken ct = default)
        => await _context.WorkoutSessions
            .Include(ws => ws.Sets.OrderBy(s => s.SetNumber))
            .FirstOrDefaultAsync(ws => ws.Id == id, ct);

    public async Task AddAsync(WorkoutSession session, CancellationToken ct = default)
        => await _context.WorkoutSessions.AddAsync(session, ct);

    public void Update(WorkoutSession session) => _context.WorkoutSessions.Update(session);

    public void Delete(WorkoutSession session) => _context.WorkoutSessions.Remove(session);
}

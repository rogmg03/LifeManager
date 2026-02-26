using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly AppDbContext _context;
    public TimeEntryRepository(AppDbContext context) => _context = context;

    public async Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.TimeEntries
            .Include(te => te.Task).ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(te => te.Id == id, ct);

    public async Task<TimeEntry?> GetActiveTimerByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.TimeEntries
            .Include(te => te.Task).ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(te => te.UserId == userId && te.EndedAt == null, ct);

    public async Task<TimeEntry?> GetRunningByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.TimeEntries
            .Include(te => te.Task).ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(te => te.TaskId == taskId && te.EndedAt == null, ct);

    public async Task<List<TimeEntry>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.TimeEntries
            .Where(te => te.TaskId == taskId)
            .OrderByDescending(te => te.StartedAt)
            .ToListAsync(ct);

    public async Task<List<TimeEntry>> GetByDateRangeAsync(Guid userId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.TimeEntries
            .Include(te => te.Task).ThenInclude(t => t.Project)
            .Where(te => te.UserId == userId && te.StartedAt >= from && te.StartedAt <= to && te.EndedAt != null)
            .OrderByDescending(te => te.StartedAt)
            .ToListAsync(ct);

    public async Task AddAsync(TimeEntry entry, CancellationToken ct = default)
        => await _context.TimeEntries.AddAsync(entry, ct);

    public void Update(TimeEntry entry) => _context.TimeEntries.Update(entry);
    public void Delete(TimeEntry entry) => _context.TimeEntries.Remove(entry);
}

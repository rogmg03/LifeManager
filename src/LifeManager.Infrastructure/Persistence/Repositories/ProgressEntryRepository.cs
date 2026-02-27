using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ProgressEntryRepository : IProgressEntryRepository
{
    private readonly AppDbContext _context;
    public ProgressEntryRepository(AppDbContext context) => _context = context;

    public async Task<List<ProgressEntry>> GetByGoalIdAsync(Guid goalId, CancellationToken ct = default)
        => await _context.ProgressEntries
            .Where(e => e.GoalId == goalId)
            .OrderByDescending(e => e.RecordedAt)
            .ToListAsync(ct);

    public async Task<ProgressEntry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.ProgressEntries.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task AddAsync(ProgressEntry entry, CancellationToken ct = default)
        => await _context.ProgressEntries.AddAsync(entry, ct);

    public void Delete(ProgressEntry entry) => _context.ProgressEntries.Remove(entry);
}

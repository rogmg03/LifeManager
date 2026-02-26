using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ActivityEntryRepository : IActivityEntryRepository
{
    private readonly AppDbContext _context;
    public ActivityEntryRepository(AppDbContext context) => _context = context;

    public async Task<(List<ActivityEntry> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        Guid? projectId,
        ActivityType? type,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _context.ActivityEntries
            .Where(a => a.UserId == userId);

        if (projectId.HasValue)
            query = query.Where(a => a.ProjectId == projectId.Value);

        if (type.HasValue)
            query = query.Where(a => a.ActivityType == type.Value);

        if (from.HasValue)
            query = query.Where(a => a.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.CreatedAt <= to.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<List<ActivityEntry>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken ct = default)
        => await _context.ActivityEntries
            .Where(a => a.ProjectId == projectId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(ActivityEntry entry, CancellationToken ct = default)
        => await _context.ActivityEntries.AddAsync(entry, ct);
}

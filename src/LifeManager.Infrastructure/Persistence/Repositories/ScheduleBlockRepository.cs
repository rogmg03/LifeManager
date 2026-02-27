using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ScheduleBlockRepository : IScheduleBlockRepository
{
    private readonly AppDbContext _context;
    public ScheduleBlockRepository(AppDbContext context) => _context = context;

    public async Task<List<ScheduleBlock>> GetByDateRangeAsync(
        Guid userId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.ScheduleBlocks
            .Where(b => b.UserId == userId && b.StartTime < to && b.EndTime > from)
            .OrderBy(b => b.StartTime)
            .ToListAsync(ct);

    public async Task<ScheduleBlock?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.ScheduleBlocks.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task AddAsync(ScheduleBlock block, CancellationToken ct = default)
        => await _context.ScheduleBlocks.AddAsync(block, ct);

    public void Update(ScheduleBlock block)
        => _context.ScheduleBlocks.Update(block);

    public void Delete(ScheduleBlock block)
        => _context.ScheduleBlocks.Remove(block);
}

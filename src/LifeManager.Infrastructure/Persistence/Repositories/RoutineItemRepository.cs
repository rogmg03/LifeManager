using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class RoutineItemRepository : IRoutineItemRepository
{
    private readonly AppDbContext _context;
    public RoutineItemRepository(AppDbContext context) => _context = context;

    public async Task<List<RoutineItem>> GetByRoutineIdAsync(Guid routineId, CancellationToken ct = default)
        => await _context.RoutineItems
            .Where(ri => ri.RoutineId == routineId)
            .OrderBy(ri => ri.SortOrder)
            .ToListAsync(ct);

    public async Task<RoutineItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.RoutineItems.FirstOrDefaultAsync(ri => ri.Id == id, ct);

    public async Task AddAsync(RoutineItem item, CancellationToken ct = default)
        => await _context.RoutineItems.AddAsync(item, ct);

    public void Update(RoutineItem item) => _context.RoutineItems.Update(item);

    public void Delete(RoutineItem item) => _context.RoutineItems.Remove(item);
}

using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class RoutineRepository : IRoutineRepository
{
    private readonly AppDbContext _context;
    public RoutineRepository(AppDbContext context) => _context = context;

    public async Task<List<Routine>> GetByUserIdAsync(Guid userId, bool includeArchived = false, CancellationToken ct = default)
    {
        var query = _context.Routines.Where(r => r.UserId == userId);
        if (!includeArchived)
            query = query.Where(r => !r.IsArchived);
        return await query.OrderBy(r => r.SortOrder).ToListAsync(ct);
    }

    public async Task<Routine?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Routines.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<Routine?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default)
        => await _context.Routines
            .Include(r => r.Items.OrderBy(i => i.SortOrder))
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(Routine routine, CancellationToken ct = default)
        => await _context.Routines.AddAsync(routine, ct);

    public void Update(Routine routine) => _context.Routines.Update(routine);

    public void Delete(Routine routine) => _context.Routines.Remove(routine);
}

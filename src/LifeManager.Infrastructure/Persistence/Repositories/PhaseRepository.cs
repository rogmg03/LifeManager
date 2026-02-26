using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class PhaseRepository : IPhaseRepository
{
    private readonly AppDbContext _context;
    public PhaseRepository(AppDbContext context) => _context = context;

    public async Task<List<Phase>> GetAllByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.Phases
            .Where(p => p.ProjectId == projectId)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(ct);

    public async Task<Phase?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Phases.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Phase phase, CancellationToken ct = default)
        => await _context.Phases.AddAsync(phase, ct);

    public void Update(Phase phase) => _context.Phases.Update(phase);
    public void Delete(Phase phase) => _context.Phases.Remove(phase);
}

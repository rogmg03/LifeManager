using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class WorkInitiativeDetailRepository : IWorkInitiativeDetailRepository
{
    private readonly AppDbContext _context;
    public WorkInitiativeDetailRepository(AppDbContext context) => _context = context;

    public async Task<WorkInitiativeDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.WorkInitiativeDetails.FirstOrDefaultAsync(d => d.ProjectId == projectId, ct);

    public async Task AddAsync(WorkInitiativeDetail detail, CancellationToken ct = default)
        => await _context.WorkInitiativeDetails.AddAsync(detail, ct);

    public void Update(WorkInitiativeDetail detail) => _context.WorkInitiativeDetails.Update(detail);
    public void Delete(WorkInitiativeDetail detail) => _context.WorkInitiativeDetails.Remove(detail);
}

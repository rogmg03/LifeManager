using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly AppDbContext _context;
    public ProjectTaskRepository(AppDbContext context) => _context = context;

    public async Task<List<ProjectTask>> GetAllByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.SortOrder)
            .ToListAsync(ct);

    public async Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task AddAsync(ProjectTask task, CancellationToken ct = default)
        => await _context.Tasks.AddAsync(task, ct);

    public void Update(ProjectTask task) => _context.Tasks.Update(task);
    public void Delete(ProjectTask task) => _context.Tasks.Remove(task);
}

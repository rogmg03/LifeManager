using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;
    public ProjectRepository(AppDbContext context) => _context = context;

    public async Task<List<Project>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.Projects.Where(p => p.UserId == userId).ToListAsync(ct);

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Project project, CancellationToken ct = default)
        => await _context.Projects.AddAsync(project, ct);

    public void Update(Project project) => _context.Projects.Update(project);
    public void Delete(Project project) => _context.Projects.Remove(project);
}

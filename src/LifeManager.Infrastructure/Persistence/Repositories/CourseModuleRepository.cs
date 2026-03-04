using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class CourseModuleRepository : ICourseModuleRepository
{
    private readonly AppDbContext _context;
    public CourseModuleRepository(AppDbContext context) => _context = context;

    public async Task<List<CourseModule>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.CourseModules
            .Where(m => m.ProjectId == projectId)
            .OrderBy(m => m.SortOrder)
            .ToListAsync(ct);

    public async Task<CourseModule?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.CourseModules.FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task AddAsync(CourseModule module, CancellationToken ct = default)
        => await _context.CourseModules.AddAsync(module, ct);

    public void Update(CourseModule module) => _context.CourseModules.Update(module);
    public void Delete(CourseModule module) => _context.CourseModules.Remove(module);
}

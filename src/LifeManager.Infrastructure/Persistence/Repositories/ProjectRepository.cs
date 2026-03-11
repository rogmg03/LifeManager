using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using TaskStatus = LifeManager.Domain.Enums.TaskStatus;

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

    public async Task<IReadOnlyDictionary<Guid, (int TotalTasks, int CompletedTasks, int OverdueTasks, int TotalTimeTrackedMinutes)>>
        GetTaskCountsAsync(IEnumerable<Guid> projectIds, CancellationToken ct = default)
    {
        var ids = projectIds.ToArray();
        if (ids.Length == 0)
            return new Dictionary<Guid, (int, int, int, int)>();

        var now = DateTime.UtcNow;

        var taskRows = await _context.Tasks
            .Where(t => ids.Contains(t.ProjectId))
            .GroupBy(t => t.ProjectId)
            .Select(g => new
            {
                ProjectId = g.Key,
                TotalTasks = g.Count(),
                CompletedTasks = g.Count(t => t.Status == TaskStatus.Done),
                OverdueTasks = g.Count(t => t.Status != TaskStatus.Done && t.DueDate != null && t.DueDate < now),
            })
            .ToListAsync(ct);

        var timeRows = await _context.TimeEntries
            .Join(_context.Tasks.Where(t => ids.Contains(t.ProjectId)),
                  te => te.TaskId, t => t.Id,
                  (te, t) => new { t.ProjectId, te.DurationMinutes })
            .GroupBy(x => x.ProjectId)
            .Select(g => new { ProjectId = g.Key, Minutes = g.Sum(x => x.DurationMinutes) ?? 0 })
            .ToListAsync(ct);

        var timeMap = timeRows.ToDictionary(r => r.ProjectId, r => r.Minutes);

        return taskRows.ToDictionary(
            r => r.ProjectId,
            r => (r.TotalTasks, r.CompletedTasks, r.OverdueTasks,
                  timeMap.GetValueOrDefault(r.ProjectId, 0)));
    }
}

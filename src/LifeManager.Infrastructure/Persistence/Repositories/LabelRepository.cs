using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly AppDbContext _context;
    public LabelRepository(AppDbContext context) => _context = context;

    // Label CRUD
    public async Task<List<Label>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.Labels.Where(l => l.UserId == userId).OrderBy(l => l.Name).ToListAsync(ct);

    public async Task<Label?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Labels.FirstOrDefaultAsync(l => l.Id == id, ct);

    public async Task AddAsync(Label label, CancellationToken ct = default)
        => await _context.Labels.AddAsync(label, ct);

    public void Update(Label label) => _context.Labels.Update(label);

    public void Delete(Label label) => _context.Labels.Remove(label);

    // Project label associations
    public async Task<List<Label>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        => await _context.ProjectLabels
            .Where(pl => pl.ProjectId == projectId)
            .Select(pl => pl.Label)
            .OrderBy(l => l.Name)
            .ToListAsync(ct);

    public async Task AddProjectLabelAsync(Guid projectId, Guid labelId, CancellationToken ct = default)
    {
        var exists = await _context.ProjectLabels
            .AnyAsync(pl => pl.ProjectId == projectId && pl.LabelId == labelId, ct);
        if (!exists)
            await _context.ProjectLabels.AddAsync(
                new ProjectLabel { ProjectId = projectId, LabelId = labelId }, ct);
    }

    public async Task RemoveProjectLabelAsync(Guid projectId, Guid labelId, CancellationToken ct = default)
    {
        var entity = await _context.ProjectLabels
            .FirstOrDefaultAsync(pl => pl.ProjectId == projectId && pl.LabelId == labelId, ct);
        if (entity is not null)
            _context.ProjectLabels.Remove(entity);
    }

    // Task label associations
    public async Task<List<Label>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.TaskLabels
            .Where(tl => tl.TaskId == taskId)
            .Select(tl => tl.Label)
            .OrderBy(l => l.Name)
            .ToListAsync(ct);

    public async Task AddTaskLabelAsync(Guid taskId, Guid labelId, CancellationToken ct = default)
    {
        var exists = await _context.TaskLabels
            .AnyAsync(tl => tl.TaskId == taskId && tl.LabelId == labelId, ct);
        if (!exists)
            await _context.TaskLabels.AddAsync(
                new TaskLabel { TaskId = taskId, LabelId = labelId }, ct);
    }

    public async Task RemoveTaskLabelAsync(Guid taskId, Guid labelId, CancellationToken ct = default)
    {
        var entity = await _context.TaskLabels
            .FirstOrDefaultAsync(tl => tl.TaskId == taskId && tl.LabelId == labelId, ct);
        if (entity is not null)
            _context.TaskLabels.Remove(entity);
    }
}

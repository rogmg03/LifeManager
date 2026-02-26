using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class SubtaskRepository : ISubtaskRepository
{
    private readonly AppDbContext _context;
    public SubtaskRepository(AppDbContext context) => _context = context;

    public async Task<List<Subtask>> GetAllByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.Subtasks
            .Where(s => s.TaskId == taskId)
            .OrderBy(s => s.SortOrder)
            .ToListAsync(ct);

    public async Task<Subtask?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Subtasks.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddAsync(Subtask subtask, CancellationToken ct = default)
        => await _context.Subtasks.AddAsync(subtask, ct);

    public void Update(Subtask subtask) => _context.Subtasks.Update(subtask);
    public void Delete(Subtask subtask) => _context.Subtasks.Remove(subtask);
}

using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class RecurrenceRuleRepository : IRecurrenceRuleRepository
{
    private readonly AppDbContext _context;
    public RecurrenceRuleRepository(AppDbContext context) => _context = context;

    public async Task<RecurrenceRule?> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _context.RecurrenceRules.FirstOrDefaultAsync(r => r.TaskId == taskId, ct);

    public async Task<RecurrenceRule?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.RecurrenceRules.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(RecurrenceRule rule, CancellationToken ct = default)
        => await _context.RecurrenceRules.AddAsync(rule, ct);

    public void Update(RecurrenceRule rule) => _context.RecurrenceRules.Update(rule);
    public void Delete(RecurrenceRule rule) => _context.RecurrenceRules.Remove(rule);
}

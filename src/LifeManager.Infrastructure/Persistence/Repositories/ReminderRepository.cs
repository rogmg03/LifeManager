using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly AppDbContext _context;
    public ReminderRepository(AppDbContext context) => _context = context;

    public async Task<List<Reminder>> GetByUserIdAsync(Guid userId, bool pendingOnly, CancellationToken ct = default)
    {
        var query = _context.Reminders.Where(r => r.UserId == userId);
        if (pendingOnly)
            query = query.Where(r => r.Status == ReminderStatus.Pending);
        return await query.OrderBy(r => r.RemindAt).ToListAsync(ct);
    }

    public async Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(Reminder reminder, CancellationToken ct = default)
        => await _context.Reminders.AddAsync(reminder, ct);

    public void Update(Reminder reminder) => _context.Reminders.Update(reminder);
    public void Delete(Reminder reminder) => _context.Reminders.Remove(reminder);
}

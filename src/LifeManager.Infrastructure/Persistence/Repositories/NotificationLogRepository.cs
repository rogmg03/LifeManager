using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class NotificationLogRepository : INotificationLogRepository
{
    private readonly AppDbContext _context;
    public NotificationLogRepository(AppDbContext context) => _context = context;

    public async Task<(List<NotificationLog> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.NotificationLogs
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.NotificationLogs.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task AddAsync(NotificationLog log, CancellationToken ct = default)
        => await _context.NotificationLogs.AddAsync(log, ct);

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
        => await _context.NotificationLogs.CountAsync(n => n.UserId == userId && !n.IsRead, ct);

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default)
        => await _context.NotificationLogs
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);

    public void Update(NotificationLog log) => _context.NotificationLogs.Update(log);

    public void Delete(NotificationLog log) => _context.NotificationLogs.Remove(log);
}

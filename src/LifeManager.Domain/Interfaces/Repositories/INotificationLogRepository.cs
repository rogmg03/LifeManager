using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface INotificationLogRepository
{
    Task<(List<NotificationLog> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task<NotificationLog?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(NotificationLog log, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);
    void Update(NotificationLog log);
    void Delete(NotificationLog log);
}

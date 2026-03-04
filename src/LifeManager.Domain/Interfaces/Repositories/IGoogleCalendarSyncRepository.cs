using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IGoogleCalendarSyncRepository
{
    Task<IReadOnlyList<GoogleCalendarSync>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<GoogleCalendarSync?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<GoogleCalendarSync?> GetByUserAndCalendarAsync(Guid userId, string calendarId, CancellationToken ct = default);
    Task AddAsync(GoogleCalendarSync sync, CancellationToken ct = default);
    void Update(GoogleCalendarSync sync);
    void Delete(GoogleCalendarSync sync);
}

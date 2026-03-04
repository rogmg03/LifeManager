using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class GoogleCalendarSyncRepository : IGoogleCalendarSyncRepository
{
    private readonly AppDbContext _context;
    public GoogleCalendarSyncRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<GoogleCalendarSync>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.GoogleCalendarSyncs
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.CalendarId)
            .ToListAsync(ct);

    public async Task<GoogleCalendarSync?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.GoogleCalendarSyncs
            .FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task<GoogleCalendarSync?> GetByUserAndCalendarAsync(Guid userId, string calendarId, CancellationToken ct = default)
        => await _context.GoogleCalendarSyncs
            .FirstOrDefaultAsync(g => g.UserId == userId && g.CalendarId == calendarId, ct);

    public async Task AddAsync(GoogleCalendarSync sync, CancellationToken ct = default)
        => await _context.GoogleCalendarSyncs.AddAsync(sync, ct);

    public void Update(GoogleCalendarSync sync)
        => _context.GoogleCalendarSyncs.Update(sync);

    public void Delete(GoogleCalendarSync sync)
        => _context.GoogleCalendarSyncs.Remove(sync);
}

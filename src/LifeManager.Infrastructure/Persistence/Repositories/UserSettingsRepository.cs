using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class UserSettingsRepository : IUserSettingsRepository
{
    private readonly AppDbContext _context;
    public UserSettingsRepository(AppDbContext context) => _context = context;

    public async Task<UserSettings?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId, ct);

    public void Update(UserSettings settings) => _context.UserSettings.Update(settings);
}

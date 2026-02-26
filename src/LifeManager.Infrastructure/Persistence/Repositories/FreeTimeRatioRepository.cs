using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class FreeTimeRatioRepository : IFreeTimeRatioRepository
{
    private readonly AppDbContext _context;
    public FreeTimeRatioRepository(AppDbContext context) => _context = context;

    public async Task<FreeTimeRatio?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.FreeTimeRatios.FirstOrDefaultAsync(r => r.UserId == userId, ct);

    public async Task AddAsync(FreeTimeRatio ratio, CancellationToken ct = default)
        => await _context.FreeTimeRatios.AddAsync(ratio, ct);

    public void Update(FreeTimeRatio ratio)
        => _context.FreeTimeRatios.Update(ratio);
}

using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class FreeTimeTransactionRepository : IFreeTimeTransactionRepository
{
    private readonly AppDbContext _context;
    public FreeTimeTransactionRepository(AppDbContext context) => _context = context;

    public async Task<FreeTimeTransaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.FreeTimeTransactions.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<int> GetLatestBalanceForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var latest = await _context.FreeTimeTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(ct);
        return latest?.BalanceAfterMinutes ?? 0;
    }

    public async Task<(List<FreeTimeTransaction> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId, DateTime? from, DateTime? to, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.FreeTimeTransactions
            .Where(t => t.UserId == userId);

        if (from.HasValue)
            query = query.Where(t => t.CreatedAt >= from.Value);
        if (to.HasValue)
            query = query.Where(t => t.CreatedAt <= to.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddAsync(FreeTimeTransaction transaction, CancellationToken ct = default)
        => await _context.FreeTimeTransactions.AddAsync(transaction, ct);

    public void Update(FreeTimeTransaction transaction)
        => _context.FreeTimeTransactions.Update(transaction);
}

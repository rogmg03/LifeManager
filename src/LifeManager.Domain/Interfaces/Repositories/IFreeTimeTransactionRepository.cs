using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IFreeTimeTransactionRepository
{
    Task<FreeTimeTransaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<int> GetLatestBalanceForUserAsync(Guid userId, CancellationToken ct = default);
    Task<(List<FreeTimeTransaction> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId, DateTime? from, DateTime? to, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(FreeTimeTransaction transaction, CancellationToken ct = default);
    void Update(FreeTimeTransaction transaction);
}

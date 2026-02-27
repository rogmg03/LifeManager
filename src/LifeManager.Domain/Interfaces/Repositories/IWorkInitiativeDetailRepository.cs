using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IWorkInitiativeDetailRepository
{
    Task<WorkInitiativeDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task AddAsync(WorkInitiativeDetail detail, CancellationToken ct = default);
    void Update(WorkInitiativeDetail detail);
    void Delete(WorkInitiativeDetail detail);
}

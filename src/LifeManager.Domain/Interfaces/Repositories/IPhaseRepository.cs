using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IPhaseRepository
{
    Task<List<Phase>> GetAllByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<Phase?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Phase phase, CancellationToken ct = default);
    void Update(Phase phase);
    void Delete(Phase phase);
}

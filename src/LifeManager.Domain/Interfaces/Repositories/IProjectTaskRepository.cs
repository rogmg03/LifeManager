using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IProjectTaskRepository
{
    Task<List<ProjectTask>> GetAllByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(ProjectTask task, CancellationToken ct = default);
    void Update(ProjectTask task);
    void Delete(ProjectTask task);
}

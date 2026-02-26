using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Project project, CancellationToken ct = default);
    void Update(Project project);
    void Delete(Project project);
}

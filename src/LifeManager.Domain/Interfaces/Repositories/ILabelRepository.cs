using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface ILabelRepository
{
    // Label CRUD
    Task<List<Label>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Label?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Label label, CancellationToken ct = default);
    void Update(Label label);
    void Delete(Label label);

    // Project label associations
    Task<List<Label>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task AddProjectLabelAsync(Guid projectId, Guid labelId, CancellationToken ct = default);
    Task RemoveProjectLabelAsync(Guid projectId, Guid labelId, CancellationToken ct = default);

    // Task label associations
    Task<List<Label>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task AddTaskLabelAsync(Guid taskId, Guid labelId, CancellationToken ct = default);
    Task RemoveTaskLabelAsync(Guid taskId, Guid labelId, CancellationToken ct = default);
}

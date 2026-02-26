using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface ISubtaskRepository
{
    Task<List<Subtask>> GetAllByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task<Subtask?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Subtask subtask, CancellationToken ct = default);
    void Update(Subtask subtask);
    void Delete(Subtask subtask);
}

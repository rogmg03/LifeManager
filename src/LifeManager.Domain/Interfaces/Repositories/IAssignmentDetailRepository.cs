using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IAssignmentDetailRepository
{
    Task<AssignmentDetail?> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task AddAsync(AssignmentDetail detail, CancellationToken ct = default);
    void Update(AssignmentDetail detail);
    void Delete(AssignmentDetail detail);
}

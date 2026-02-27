using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface IOnlineCourseDetailRepository
{
    Task<OnlineCourseDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task AddAsync(OnlineCourseDetail detail, CancellationToken ct = default);
    void Update(OnlineCourseDetail detail);
    void Delete(OnlineCourseDetail detail);
}

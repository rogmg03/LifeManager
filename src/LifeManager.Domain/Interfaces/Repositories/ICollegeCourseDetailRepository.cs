using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface ICollegeCourseDetailRepository
{
    Task<CollegeCourseDetail?> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task AddAsync(CollegeCourseDetail detail, CancellationToken ct = default);
    void Update(CollegeCourseDetail detail);
    void Delete(CollegeCourseDetail detail);
}

using LifeManager.Domain.Entities;

namespace LifeManager.Domain.Interfaces.Repositories;

public interface ICourseModuleRepository
{
    Task<List<CourseModule>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<CourseModule?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(CourseModule module, CancellationToken ct = default);
    void Update(CourseModule module);
    void Delete(CourseModule module);
}

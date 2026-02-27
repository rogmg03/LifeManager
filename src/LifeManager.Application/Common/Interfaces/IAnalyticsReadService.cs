using LifeManager.Application.Features.Analytics.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface IAnalyticsReadService
{
    Task<ProductivityDto> GetProductivityAsync(Guid userId, string period, CancellationToken ct = default);
    Task<AcademicAnalyticsDto?> GetAcademicAsync(Guid userId, Guid projectId, CancellationToken ct = default);
    Task<ExerciseAnalyticsDto?> GetExerciseAsync(Guid userId, Guid projectId, CancellationToken ct = default);
    Task<ProjectHealthDto> GetProjectHealthAsync(Guid userId, CancellationToken ct = default);
    Task<TimeTrackingAnalyticsDto> GetTimeTrackingAsync(Guid userId, string period, string groupBy, CancellationToken ct = default);
}

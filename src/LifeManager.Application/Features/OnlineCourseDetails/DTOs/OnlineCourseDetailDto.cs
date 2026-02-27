using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.OnlineCourseDetails.DTOs;

public record OnlineCourseDetailDto(
    Guid ProjectId,
    string? Platform,
    string? CourseUrl,
    string? InstructorName,
    int? TotalLessons,
    int? CompletedLessons,
    string? CertificateUrl,
    DateOnly? StartedAt,
    DateOnly? CompletedAt)
{
    public static OnlineCourseDetailDto FromEntity(OnlineCourseDetail d) => new(
        d.ProjectId,
        d.Platform,
        d.CourseUrl,
        d.InstructorName,
        d.TotalLessons,
        d.CompletedLessons,
        d.CertificateUrl,
        d.StartedAt,
        d.CompletedAt);
}

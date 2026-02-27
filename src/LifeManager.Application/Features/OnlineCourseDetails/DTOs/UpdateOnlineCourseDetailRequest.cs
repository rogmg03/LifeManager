namespace LifeManager.Application.Features.OnlineCourseDetails.DTOs;

public record UpdateOnlineCourseDetailRequest(
    string? Platform,
    string? CourseUrl,
    string? InstructorName,
    int? TotalLessons,
    int? CompletedLessons,
    string? CertificateUrl,
    DateOnly? StartedAt,
    DateOnly? CompletedAt);

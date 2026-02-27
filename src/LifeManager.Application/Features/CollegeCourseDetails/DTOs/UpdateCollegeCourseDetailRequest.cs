namespace LifeManager.Application.Features.CollegeCourseDetails.DTOs;

public record UpdateCollegeCourseDetailRequest(
    string? InstitutionName,
    string? CourseName,
    string? CourseCode,
    string? Semester,
    int? Year,
    decimal? Credits,
    string? Professor,
    string? Room,
    string? Schedule,
    decimal? CurrentGrade,
    decimal? TargetGrade);

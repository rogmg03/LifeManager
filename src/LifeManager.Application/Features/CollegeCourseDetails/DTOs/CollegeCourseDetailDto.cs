using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.CollegeCourseDetails.DTOs;

public record CollegeCourseDetailDto(
    Guid ProjectId,
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
    decimal? TargetGrade)
{
    public static CollegeCourseDetailDto FromEntity(CollegeCourseDetail d) => new(
        d.ProjectId,
        d.InstitutionName,
        d.CourseName,
        d.CourseCode,
        d.Semester,
        d.Year,
        d.Credits,
        d.Professor,
        d.Room,
        d.Schedule,
        d.CurrentGrade,
        d.TargetGrade);
}

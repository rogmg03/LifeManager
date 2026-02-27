namespace LifeManager.Domain.Entities;

public class OnlineCourseDetail
{
    public Guid ProjectId { get; set; }
    public string? Platform { get; set; }
    public string? CourseUrl { get; set; }
    public string? InstructorName { get; set; }
    public int? TotalLessons { get; set; }
    public int? CompletedLessons { get; set; }
    public string? CertificateUrl { get; set; }
    public DateOnly? StartedAt { get; set; }
    public DateOnly? CompletedAt { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}

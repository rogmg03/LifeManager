namespace LifeManager.Domain.Entities;

public class CollegeCourseDetail
{
    public Guid ProjectId { get; set; }
    public string? InstitutionName { get; set; }
    public string? CourseName { get; set; }
    public string? CourseCode { get; set; }
    public string? Semester { get; set; }
    public int? Year { get; set; }
    public decimal? Credits { get; set; }
    public string? Professor { get; set; }
    public string? Room { get; set; }
    public string? Schedule { get; set; }
    public decimal? CurrentGrade { get; set; }
    public decimal? TargetGrade { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}

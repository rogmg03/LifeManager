using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class AssignmentDetail
{
    public Guid TaskId { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public decimal? Grade { get; set; }
    public string? GradeLetter { get; set; }
    public decimal? Weight { get; set; }
    public string? SubmissionLink { get; set; }

    // Navigation
    public ProjectTask Task { get; set; } = null!;
}

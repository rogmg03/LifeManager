namespace LifeManager.Application.Features.Analytics.DTOs;

public class AcademicAnalyticsDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public decimal? CurrentGrade { get; set; }
    public decimal? TargetGrade { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int PendingTasks { get; set; }
    public IEnumerable<PriorityCountDto> TasksByPriority { get; set; } = [];
    public IEnumerable<RecentCompletionDto> RecentCompletions { get; set; } = [];
}

public class PriorityCountDto
{
    public string Priority { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class RecentCompletionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }
}

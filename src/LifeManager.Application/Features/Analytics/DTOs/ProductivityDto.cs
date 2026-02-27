namespace LifeManager.Application.Features.Analytics.DTOs;

public class ProductivityDto
{
    public IEnumerable<DailyTaskCountDto> TasksCompletedByDate { get; set; } = [];
    public IEnumerable<ProjectTimeDto> TimeTrackedByProject { get; set; } = [];
    public int TotalTasksCompleted { get; set; }
    public int OverdueCompletedCount { get; set; }
    public int OnTimeCompletedCount { get; set; }
}

public class DailyTaskCountDto
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class ProjectTimeDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int TotalMinutes { get; set; }
}

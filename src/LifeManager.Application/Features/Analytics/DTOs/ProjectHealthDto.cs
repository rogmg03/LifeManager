namespace LifeManager.Application.Features.Analytics.DTOs;

public class ProjectHealthDto
{
    public int TotalActiveProjects { get; set; }
    public int TotalOverdueTasksAcrossAll { get; set; }
    public IEnumerable<ProjectHealthItemDto> Projects { get; set; } = [];
}

public class ProjectHealthItemDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TaskCount { get; set; }
    public int CompletedCount { get; set; }
    public int OverdueCount { get; set; }
    public int CompletionPercent => TaskCount == 0 ? 0 : (int)((double)CompletedCount / TaskCount * 100);
}

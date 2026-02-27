namespace LifeManager.Application.Features.Analytics.DTOs;

public class ExerciseAnalyticsDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int WorkoutLogsThisWeek { get; set; }
    public IEnumerable<WorkoutWeekDto> WorkoutLogsByWeek { get; set; } = [];
    public IEnumerable<GoalProgressDto> Goals { get; set; } = [];
}

public class WorkoutWeekDto
{
    public DateTime WeekStart { get; set; }
    public int Count { get; set; }
}

public class GoalProgressDto
{
    public Guid GoalId { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
    public decimal? LatestValue { get; set; }
    public decimal? PersonalBest { get; set; }
}

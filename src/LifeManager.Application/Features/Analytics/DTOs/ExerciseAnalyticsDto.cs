namespace LifeManager.Application.Features.Analytics.DTOs;

public class ExerciseAnalyticsDto
{
    public int WorkoutSessionsThisWeek { get; set; }
    public IEnumerable<WorkoutWeekDto> WorkoutSessionsByWeek { get; set; } = [];
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

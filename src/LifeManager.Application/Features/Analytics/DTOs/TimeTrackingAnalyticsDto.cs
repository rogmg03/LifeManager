namespace LifeManager.Application.Features.Analytics.DTOs;

public class TimeTrackingAnalyticsDto
{
    public int TotalMinutes { get; set; }
    public IEnumerable<TimeTrackingEntryDto> Entries { get; set; } = [];
}

public class TimeTrackingEntryDto
{
    public string Dimension { get; set; } = string.Empty;
    public int TotalMinutes { get; set; }
}

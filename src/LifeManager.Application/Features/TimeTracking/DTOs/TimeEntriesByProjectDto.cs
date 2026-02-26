namespace LifeManager.Application.Features.TimeTracking.DTOs;

public record TimeEntriesByProjectDto(
    Guid ProjectId,
    string ProjectName,
    int TotalMinutes,
    List<TimeEntryDto> Entries);

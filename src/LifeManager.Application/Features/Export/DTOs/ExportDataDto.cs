namespace LifeManager.Application.Features.Export.DTOs;

public class ExportDataDto
{
    public DateTime ExportedAt { get; init; } = DateTime.UtcNow;
    public List<ExportProjectRow> Projects { get; init; } = [];
    public List<ExportTaskRow> Tasks { get; init; } = [];
    public List<ExportClientRow> Clients { get; init; } = [];
    public List<ExportTimeEntryRow> TimeEntries { get; init; } = [];
    public List<ExportScheduleBlockRow> ScheduleBlocks { get; init; } = [];
    public List<ExportLabelRow> Labels { get; init; } = [];
    public List<ExportDocumentRow> Documents { get; init; } = [];
}

public record ExportProjectRow(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    string Status,
    string? Color,
    DateTime? StartDate,
    DateTime? EndDate,
    DateTime CreatedAt);

public record ExportTaskRow(
    Guid Id,
    Guid ProjectId,
    string ProjectName,
    string Title,
    string? Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    int? EstimatedMinutes,
    DateTime? CompletedAt,
    DateTime CreatedAt);

public record ExportClientRow(
    Guid Id,
    string Name,
    string? ContactPerson,
    string Priority,
    string Status,
    string? Notes,
    string Color,
    DateTime CreatedAt);

public record ExportTimeEntryRow(
    Guid Id,
    Guid TaskId,
    string TaskTitle,
    string ProjectName,
    DateTime StartedAt,
    DateTime? EndedAt,
    int? DurationMinutes,
    string? Notes,
    DateTime CreatedAt);

public record ExportScheduleBlockRow(
    Guid Id,
    string Title,
    string? Description,
    string BlockType,
    string Status,
    DateTime StartTime,
    DateTime EndTime,
    DateTime CreatedAt);

public record ExportLabelRow(
    Guid Id,
    string Name,
    string Color,
    DateTime CreatedAt);

public record ExportDocumentRow(
    Guid Id,
    Guid ProjectId,
    string ProjectName,
    string Title,
    string? ContentSnippet,
    DateTime CreatedAt,
    DateTime UpdatedAt);

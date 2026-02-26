using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Documents.DTOs;

public record AttachmentSummaryDto(
    Guid Id,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    DateTime CreatedAt);

public record DocumentDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Content,
    int WordCount,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<AttachmentSummaryDto> Attachments)
{
    public static DocumentDto FromEntity(Document d) => new(
        d.Id,
        d.ProjectId,
        d.Title,
        d.Content,
        d.WordCount,
        d.CreatedAt,
        d.UpdatedAt,
        d.Attachments.Select(a => new AttachmentSummaryDto(
            a.Id, a.FileName, a.ContentType, a.FileSizeBytes, a.CreatedAt)).ToList());

    public static DocumentDto FromEntityNoAttachments(Document d) => new(
        d.Id,
        d.ProjectId,
        d.Title,
        d.Content,
        d.WordCount,
        d.CreatedAt,
        d.UpdatedAt,
        Array.Empty<AttachmentSummaryDto>());
}

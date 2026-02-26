using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Attachments.DTOs;

public record AttachmentDto(
    Guid Id,
    Guid? DocumentId,
    Guid? TaskId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    DateTime CreatedAt)
{
    public static AttachmentDto FromEntity(Attachment a) => new(
        a.Id,
        a.DocumentId,
        a.TaskId,
        a.FileName,
        a.ContentType,
        a.FileSizeBytes,
        a.CreatedAt);
}

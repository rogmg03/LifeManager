using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Attachments.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Attachments.Commands;

public record UploadAttachmentCommand(
    Guid? DocumentId,
    Guid? TaskId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    Stream FileStream) : IRequest<AttachmentDto>, IBaseCommand;

public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, AttachmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _fileStorage;

    public UploadAttachmentCommandHandler(IUnitOfWork uow, IFileStorageService fileStorage)
    { _uow = uow; _fileStorage = fileStorage; }

    public async Task<AttachmentDto> Handle(UploadAttachmentCommand request, CancellationToken ct)
    {
        // Validate that exactly one owner is specified
        if (request.DocumentId is null && request.TaskId is null)
            throw new ArgumentException("Attachment must belong to a Document or a Task.");

        // Verify the owner exists
        if (request.DocumentId.HasValue)
        {
            _ = await _uow.Documents.GetByIdWithAttachmentsAsync(request.DocumentId.Value, ct)
                ?? throw new NotFoundException("Document", request.DocumentId.Value);
        }
        else if (request.TaskId.HasValue)
        {
            _ = await _uow.Tasks.GetByIdAsync(request.TaskId.Value, ct)
                ?? throw new NotFoundException("Task", request.TaskId.Value);
        }

        var storagePath = await _fileStorage.SaveFileAsync(
            request.FileStream, request.FileName, request.ContentType, ct);

        var attachment = new Attachment
        {
            DocumentId = request.DocumentId,
            TaskId = request.TaskId,
            FileName = request.FileName,
            StoragePath = storagePath,
            ContentType = request.ContentType,
            FileSizeBytes = request.FileSizeBytes
        };

        await _uow.Attachments.AddAsync(attachment, ct);
        await _uow.SaveChangesAsync(ct);
        return AttachmentDto.FromEntity(attachment);
    }
}

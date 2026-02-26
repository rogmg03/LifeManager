using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Attachments.Queries;

public record AttachmentDownloadResult(
    Stream FileStream,
    string FileName,
    string ContentType,
    long FileSizeBytes);

public record DownloadAttachmentQuery(Guid Id) : IRequest<AttachmentDownloadResult>;

public class DownloadAttachmentQueryHandler : IRequestHandler<DownloadAttachmentQuery, AttachmentDownloadResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _fileStorage;

    public DownloadAttachmentQueryHandler(IUnitOfWork uow, IFileStorageService fileStorage)
    { _uow = uow; _fileStorage = fileStorage; }

    public async Task<AttachmentDownloadResult> Handle(DownloadAttachmentQuery request, CancellationToken ct)
    {
        var attachment = await _uow.Attachments.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Attachment", request.Id);

        var stream = await _fileStorage.GetFileStreamAsync(attachment.StoragePath, ct);
        return new AttachmentDownloadResult(stream, attachment.FileName, attachment.ContentType, attachment.FileSizeBytes);
    }
}

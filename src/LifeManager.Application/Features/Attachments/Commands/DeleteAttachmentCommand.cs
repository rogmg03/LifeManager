using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Attachments.Commands;

public record DeleteAttachmentCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _fileStorage;

    public DeleteAttachmentCommandHandler(IUnitOfWork uow, IFileStorageService fileStorage)
    { _uow = uow; _fileStorage = fileStorage; }

    public async Task Handle(DeleteAttachmentCommand request, CancellationToken ct)
    {
        var attachment = await _uow.Attachments.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Attachment", request.Id);

        await _fileStorage.DeleteFileAsync(attachment.StoragePath, ct);

        _uow.Attachments.Delete(attachment);
        await _uow.SaveChangesAsync(ct);
    }
}

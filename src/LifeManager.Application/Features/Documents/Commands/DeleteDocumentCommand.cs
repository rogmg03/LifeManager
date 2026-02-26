using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Commands;

public record DeleteDocumentCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _fileStorage;

    public DeleteDocumentCommandHandler(IUnitOfWork uow, IFileStorageService fileStorage)
    { _uow = uow; _fileStorage = fileStorage; }

    public async Task Handle(DeleteDocumentCommand request, CancellationToken ct)
    {
        var document = await _uow.Documents.GetByIdWithAttachmentsAsync(request.Id, ct)
            ?? throw new NotFoundException("Document", request.Id);

        // Delete each attachment's stored file first
        foreach (var attachment in document.Attachments)
            await _fileStorage.DeleteFileAsync(attachment.StoragePath, ct);

        // EF cascade will remove attachment rows when document is deleted
        _uow.Documents.Delete(document);
        await _uow.SaveChangesAsync(ct);
    }
}

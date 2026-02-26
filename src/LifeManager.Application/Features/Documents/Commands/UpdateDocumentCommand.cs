using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Commands;

public record UpdateDocumentCommand(
    Guid Id,
    string Title,
    string? Content) : IRequest<DocumentDto>, IBaseCommand;

public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, DocumentDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateDocumentCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DocumentDto> Handle(UpdateDocumentCommand request, CancellationToken ct)
    {
        var document = await _uow.Documents.GetByIdWithAttachmentsAsync(request.Id, ct)
            ?? throw new NotFoundException("Document", request.Id);

        document.Title = request.Title;
        document.Content = request.Content;
        document.WordCount = CountWords(request.Content);

        _uow.Documents.Update(document);
        await _uow.SaveChangesAsync(ct);
        return DocumentDto.FromEntity(document);
    }

    private static int CountWords(string? content)
        => string.IsNullOrWhiteSpace(content)
            ? 0
            : content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}

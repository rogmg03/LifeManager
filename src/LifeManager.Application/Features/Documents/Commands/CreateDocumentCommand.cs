using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Commands;

public record CreateDocumentCommand(
    Guid ProjectId,
    string Title,
    string? Content) : IRequest<DocumentDto>, IBaseCommand;

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, DocumentDto>
{
    private readonly IUnitOfWork _uow;

    public CreateDocumentCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DocumentDto> Handle(CreateDocumentCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        var wordCount = CountWords(request.Content);

        var document = new Document
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Content = request.Content,
            WordCount = wordCount
        };

        await _uow.Documents.AddAsync(document, ct);
        await _uow.SaveChangesAsync(ct);
        return DocumentDto.FromEntityNoAttachments(document);
    }

    private static int CountWords(string? content)
        => string.IsNullOrWhiteSpace(content)
            ? 0
            : content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}

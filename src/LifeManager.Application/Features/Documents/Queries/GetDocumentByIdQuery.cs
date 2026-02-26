using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Queries;

public record GetDocumentByIdQuery(Guid Id) : IRequest<DocumentDto>;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
{
    private readonly IUnitOfWork _uow;

    public GetDocumentByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DocumentDto> Handle(GetDocumentByIdQuery request, CancellationToken ct)
    {
        var document = await _uow.Documents.GetByIdWithAttachmentsAsync(request.Id, ct)
            ?? throw new NotFoundException("Document", request.Id);
        return DocumentDto.FromEntity(document);
    }
}

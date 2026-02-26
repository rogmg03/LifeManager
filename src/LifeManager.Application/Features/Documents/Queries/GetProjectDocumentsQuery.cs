using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Queries;

public record GetProjectDocumentsQuery(Guid ProjectId) : IRequest<List<DocumentDto>>;

public class GetProjectDocumentsQueryHandler : IRequestHandler<GetProjectDocumentsQuery, List<DocumentDto>>
{
    private readonly IUnitOfWork _uow;

    public GetProjectDocumentsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<DocumentDto>> Handle(GetProjectDocumentsQuery request, CancellationToken ct)
    {
        var documents = await _uow.Documents.GetByProjectIdAsync(request.ProjectId, ct);
        return documents.Select(DocumentDto.FromEntityNoAttachments).ToList();
    }
}

using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Documents.Queries;

public record SearchDocumentsQuery(string Query) : IRequest<List<DocumentDto>>;

public class SearchDocumentsQueryHandler : IRequestHandler<SearchDocumentsQuery, List<DocumentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public SearchDocumentsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<DocumentDto>> Handle(SearchDocumentsQuery request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new List<DocumentDto>();

        var documents = await _uow.Documents.SearchAsync(_currentUser.UserId, request.Query, ct);
        return documents.Select(DocumentDto.FromEntity).ToList();
    }
}

using LifeManager.Application.Features.ProgressEntries.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ProgressEntries.Queries;

public record GetProgressEntriesByGoalQuery(Guid GoalId) : IRequest<List<ProgressEntryDto>>;

public class GetProgressEntriesByGoalQueryHandler : IRequestHandler<GetProgressEntriesByGoalQuery, List<ProgressEntryDto>>
{
    private readonly IUnitOfWork _uow;
    public GetProgressEntriesByGoalQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<ProgressEntryDto>> Handle(GetProgressEntriesByGoalQuery request, CancellationToken ct)
    {
        var entries = await _uow.ProgressEntries.GetByGoalIdAsync(request.GoalId, ct);
        return entries.Select(ProgressEntryDto.FromEntity).ToList();
    }
}

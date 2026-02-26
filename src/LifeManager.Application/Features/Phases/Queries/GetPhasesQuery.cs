using LifeManager.Application.Features.Phases.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Phases.Queries;

public record GetPhasesQuery(Guid ProjectId) : IRequest<List<PhaseDto>>;

public class GetPhasesQueryHandler : IRequestHandler<GetPhasesQuery, List<PhaseDto>>
{
    private readonly IUnitOfWork _uow;
    public GetPhasesQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<PhaseDto>> Handle(GetPhasesQuery request, CancellationToken ct)
    {
        var phases = await _uow.Phases.GetAllByProjectIdAsync(request.ProjectId, ct);
        return phases.Select(PhaseDto.FromEntity).ToList();
    }
}

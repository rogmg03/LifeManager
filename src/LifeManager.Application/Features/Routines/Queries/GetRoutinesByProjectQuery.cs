using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Queries;

public record GetRoutinesByProjectQuery(Guid ProjectId) : IRequest<List<RoutineDto>>;

public class GetRoutinesByProjectQueryHandler : IRequestHandler<GetRoutinesByProjectQuery, List<RoutineDto>>
{
    private readonly IUnitOfWork _uow;
    public GetRoutinesByProjectQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<RoutineDto>> Handle(GetRoutinesByProjectQuery request, CancellationToken ct)
    {
        var routines = await _uow.Routines.GetByProjectIdAsync(request.ProjectId, ct);
        return routines.Select(RoutineDto.FromEntity).ToList();
    }
}

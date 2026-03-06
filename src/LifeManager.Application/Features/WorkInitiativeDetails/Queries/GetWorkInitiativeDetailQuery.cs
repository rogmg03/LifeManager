using LifeManager.Application.Features.WorkInitiativeDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkInitiativeDetails.Queries;

public record GetWorkInitiativeDetailQuery(Guid ProjectId) : IRequest<WorkInitiativeDetailDto?>;

public class GetWorkInitiativeDetailQueryHandler : IRequestHandler<GetWorkInitiativeDetailQuery, WorkInitiativeDetailDto?>
{
    private readonly IUnitOfWork _uow;
    public GetWorkInitiativeDetailQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<WorkInitiativeDetailDto?> Handle(GetWorkInitiativeDetailQuery request, CancellationToken ct)
    {
        var detail = await _uow.WorkInitiativeDetails.GetByProjectIdAsync(request.ProjectId, ct);
        return detail is null ? null : WorkInitiativeDetailDto.FromEntity(detail);
    }
}

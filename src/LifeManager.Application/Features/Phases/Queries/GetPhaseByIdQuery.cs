using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Phases.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Phases.Queries;

public record GetPhaseByIdQuery(Guid Id) : IRequest<PhaseDto>;

public class GetPhaseByIdQueryHandler : IRequestHandler<GetPhaseByIdQuery, PhaseDto>
{
    private readonly IUnitOfWork _uow;
    public GetPhaseByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PhaseDto> Handle(GetPhaseByIdQuery request, CancellationToken ct)
    {
        var phase = await _uow.Phases.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Phase", request.Id);
        return PhaseDto.FromEntity(phase);
    }
}

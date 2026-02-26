using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Phases.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Phases.Commands;

public record UpdatePhaseCommand(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder,
    Priority? Priority,
    PhaseStatus Status,
    DateOnly? StartDate,
    DateOnly? EndDate) : IRequest<PhaseDto>, IBaseCommand;

public class UpdatePhaseCommandHandler : IRequestHandler<UpdatePhaseCommand, PhaseDto>
{
    private readonly IUnitOfWork _uow;
    public UpdatePhaseCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PhaseDto> Handle(UpdatePhaseCommand request, CancellationToken ct)
    {
        var phase = await _uow.Phases.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Phase", request.Id);

        phase.Name = request.Name;
        phase.Description = request.Description;
        phase.SortOrder = request.SortOrder;
        phase.Priority = request.Priority;
        phase.Status = request.Status;
        phase.StartDate = request.StartDate;
        phase.EndDate = request.EndDate;

        _uow.Phases.Update(phase);
        await _uow.SaveChangesAsync(ct);
        return PhaseDto.FromEntity(phase);
    }
}

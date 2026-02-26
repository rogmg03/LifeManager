using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.Phases.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Phases.Commands;

public record CreatePhaseCommand(
    Guid ProjectId,
    string Name,
    string? Description,
    int SortOrder,
    Priority? Priority,
    DateOnly? StartDate,
    DateOnly? EndDate) : IRequest<PhaseDto>, IBaseCommand;

public class CreatePhaseCommandHandler : IRequestHandler<CreatePhaseCommand, PhaseDto>
{
    private readonly IUnitOfWork _uow;
    public CreatePhaseCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PhaseDto> Handle(CreatePhaseCommand request, CancellationToken ct)
    {
        var phase = new Phase
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            SortOrder = request.SortOrder,
            Priority = request.Priority,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        await _uow.Phases.AddAsync(phase, ct);
        await _uow.SaveChangesAsync(ct);
        return PhaseDto.FromEntity(phase);
    }
}

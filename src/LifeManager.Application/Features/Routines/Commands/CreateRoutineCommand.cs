using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record CreateRoutineCommand(
    Guid ProjectId,
    string Name,
    string? Description,
    int? DayOfWeek,
    int SortOrder) : IRequest<RoutineDto>, IBaseCommand;

public class CreateRoutineCommandHandler : IRequestHandler<CreateRoutineCommand, RoutineDto>
{
    private readonly IUnitOfWork _uow;
    public CreateRoutineCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoutineDto> Handle(CreateRoutineCommand request, CancellationToken ct)
    {
        var routine = new Routine
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            DayOfWeek = request.DayOfWeek,
            SortOrder = request.SortOrder
        };

        await _uow.Routines.AddAsync(routine, ct);
        await _uow.SaveChangesAsync(ct);

        return RoutineDto.FromEntity(routine);
    }
}

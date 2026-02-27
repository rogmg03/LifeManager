using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record UpdateRoutineCommand(
    Guid Id,
    string Name,
    string? Description,
    int? DayOfWeek,
    int SortOrder) : IRequest<RoutineDto>, IBaseCommand;

public class UpdateRoutineCommandHandler : IRequestHandler<UpdateRoutineCommand, RoutineDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateRoutineCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoutineDto> Handle(UpdateRoutineCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Routine", request.Id);

        routine.Name = request.Name;
        routine.Description = request.Description;
        routine.DayOfWeek = request.DayOfWeek;
        routine.SortOrder = request.SortOrder;

        _uow.Routines.Update(routine);
        await _uow.SaveChangesAsync(ct);

        return RoutineDto.FromEntity(routine);
    }
}

using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record UpdateRoutineItemCommand(
    Guid Id,
    string ExerciseName,
    string? Description,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds,
    int SortOrder) : IRequest<RoutineItemDto>, IBaseCommand;

public class UpdateRoutineItemCommandHandler : IRequestHandler<UpdateRoutineItemCommand, RoutineItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateRoutineItemCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<RoutineItemDto> Handle(UpdateRoutineItemCommand request, CancellationToken ct)
    {
        var item = await _uow.RoutineItems.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("RoutineItem", request.Id);

        var routine = await _uow.Routines.GetByIdAsync(item.RoutineId, ct)
            ?? throw new NotFoundException("Routine", item.RoutineId);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("RoutineItem", request.Id);

        item.ExerciseName = request.ExerciseName;
        item.Description = request.Description;
        item.TargetSets = request.TargetSets;
        item.TargetReps = request.TargetReps;
        item.TargetWeight = request.TargetWeight;
        item.RestSeconds = request.RestSeconds;
        item.SortOrder = request.SortOrder;

        _uow.RoutineItems.Update(item);
        await _uow.SaveChangesAsync(ct);

        return RoutineItemDto.FromEntity(item);
    }
}

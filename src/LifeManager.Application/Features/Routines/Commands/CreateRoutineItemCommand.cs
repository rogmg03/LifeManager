using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record CreateRoutineItemCommand(
    Guid RoutineId,
    string ExerciseName,
    string? Description,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds,
    int SortOrder) : IRequest<RoutineItemDto>, IBaseCommand;

public class CreateRoutineItemCommandHandler : IRequestHandler<CreateRoutineItemCommand, RoutineItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateRoutineItemCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<RoutineItemDto> Handle(CreateRoutineItemCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdAsync(request.RoutineId, ct)
            ?? throw new NotFoundException("Routine", request.RoutineId);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.RoutineId);

        var item = new RoutineItem
        {
            RoutineId = request.RoutineId,
            ExerciseName = request.ExerciseName,
            Description = request.Description,
            TargetSets = request.TargetSets,
            TargetReps = request.TargetReps,
            TargetWeight = request.TargetWeight,
            RestSeconds = request.RestSeconds,
            SortOrder = request.SortOrder
        };

        await _uow.RoutineItems.AddAsync(item, ct);
        await _uow.SaveChangesAsync(ct);

        return RoutineItemDto.FromEntity(item);
    }
}

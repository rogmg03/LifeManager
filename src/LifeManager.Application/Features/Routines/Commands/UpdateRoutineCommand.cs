using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record UpdateRoutineCommand(
    Guid Id,
    string Name,
    string? Description,
    int? EstimatedDurationMinutes,
    string? Category,
    int? SortOrder) : IRequest<RoutineDto>, IBaseCommand;

public class UpdateRoutineCommandHandler : IRequestHandler<UpdateRoutineCommand, RoutineDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateRoutineCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<RoutineDto> Handle(UpdateRoutineCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdWithItemsAsync(request.Id, ct)
            ?? throw new NotFoundException("Routine", request.Id);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.Id);

        routine.Name = request.Name;
        routine.Description = request.Description;
        routine.EstimatedDurationMinutes = request.EstimatedDurationMinutes;
        routine.Category = request.Category;
        if (request.SortOrder.HasValue)
            routine.SortOrder = request.SortOrder.Value;

        _uow.Routines.Update(routine);
        await _uow.SaveChangesAsync(ct);

        return new RoutineDto(routine.Id, routine.Name, routine.Description,
            routine.EstimatedDurationMinutes, routine.Category, routine.IsArchived,
            routine.SortOrder, routine.Items.Count, null, routine.CreatedAt, routine.UpdatedAt);
    }
}

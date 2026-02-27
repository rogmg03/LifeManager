using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record CreateRoutineCommand(
    string Name,
    string? Description,
    int? EstimatedDurationMinutes,
    string? Category,
    int SortOrder) : IRequest<RoutineDto>, IBaseCommand;

public class CreateRoutineCommandHandler : IRequestHandler<CreateRoutineCommand, RoutineDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateRoutineCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<RoutineDto> Handle(CreateRoutineCommand request, CancellationToken ct)
    {
        var routine = new Routine
        {
            UserId = _currentUser.UserId,
            Name = request.Name,
            Description = request.Description,
            EstimatedDurationMinutes = request.EstimatedDurationMinutes,
            Category = request.Category,
            SortOrder = request.SortOrder
        };

        await _uow.Routines.AddAsync(routine, ct);
        await _uow.SaveChangesAsync(ct);

        return new RoutineDto(routine.Id, routine.Name, routine.Description,
            routine.EstimatedDurationMinutes, routine.Category, routine.IsArchived,
            routine.SortOrder, 0, null, routine.CreatedAt, routine.UpdatedAt);
    }
}

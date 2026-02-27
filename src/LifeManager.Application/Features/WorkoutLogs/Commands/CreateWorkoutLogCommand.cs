using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.WorkoutLogs.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkoutLogs.Commands;

public record CreateWorkoutLogCommand(
    Guid ProjectId,
    Guid? RoutineId,
    DateTimeOffset LoggedAt,
    string? Notes,
    int? DurationMinutes) : IRequest<WorkoutLogDto>, IBaseCommand;

public class CreateWorkoutLogCommandHandler : IRequestHandler<CreateWorkoutLogCommand, WorkoutLogDto>
{
    private readonly IUnitOfWork _uow;
    public CreateWorkoutLogCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<WorkoutLogDto> Handle(CreateWorkoutLogCommand request, CancellationToken ct)
    {
        var log = new WorkoutLog
        {
            ProjectId = request.ProjectId,
            RoutineId = request.RoutineId,
            LoggedAt = request.LoggedAt,
            Notes = request.Notes,
            DurationMinutes = request.DurationMinutes
        };

        await _uow.WorkoutLogs.AddAsync(log, ct);
        await _uow.SaveChangesAsync(ct);

        return WorkoutLogDto.FromEntity(log);
    }
}

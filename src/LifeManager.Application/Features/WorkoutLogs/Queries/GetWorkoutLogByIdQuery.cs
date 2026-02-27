using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.WorkoutLogs.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkoutLogs.Queries;

public record GetWorkoutLogByIdQuery(Guid Id) : IRequest<WorkoutLogDto>;

public class GetWorkoutLogByIdQueryHandler : IRequestHandler<GetWorkoutLogByIdQuery, WorkoutLogDto>
{
    private readonly IUnitOfWork _uow;
    public GetWorkoutLogByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<WorkoutLogDto> Handle(GetWorkoutLogByIdQuery request, CancellationToken ct)
    {
        var log = await _uow.WorkoutLogs.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("WorkoutLog", request.Id);

        return WorkoutLogDto.FromEntity(log);
    }
}

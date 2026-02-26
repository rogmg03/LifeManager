using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Tasks.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Tasks.Queries;

public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDto>;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly IUnitOfWork _uow;
    public GetTaskByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Task", request.Id);
        return TaskDto.FromEntity(task);
    }
}

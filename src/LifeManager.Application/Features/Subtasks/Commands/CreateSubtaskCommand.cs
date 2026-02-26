using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.Subtasks.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Subtasks.Commands;

public record CreateSubtaskCommand(Guid TaskId, string Title, int SortOrder) : IRequest<SubtaskDto>, IBaseCommand;

public class CreateSubtaskCommandHandler : IRequestHandler<CreateSubtaskCommand, SubtaskDto>
{
    private readonly IUnitOfWork _uow;
    public CreateSubtaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<SubtaskDto> Handle(CreateSubtaskCommand request, CancellationToken ct)
    {
        var subtask = new Subtask { TaskId = request.TaskId, Title = request.Title, SortOrder = request.SortOrder };
        await _uow.Subtasks.AddAsync(subtask, ct);
        await _uow.SaveChangesAsync(ct);
        return SubtaskDto.FromEntity(subtask);
    }
}

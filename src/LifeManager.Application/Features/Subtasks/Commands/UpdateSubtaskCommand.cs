using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Subtasks.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Subtasks.Commands;

public record UpdateSubtaskCommand(Guid Id, string Title, bool IsCompleted, int SortOrder) : IRequest<SubtaskDto>, IBaseCommand;

public class UpdateSubtaskCommandHandler : IRequestHandler<UpdateSubtaskCommand, SubtaskDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateSubtaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<SubtaskDto> Handle(UpdateSubtaskCommand request, CancellationToken ct)
    {
        var subtask = await _uow.Subtasks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Subtask", request.Id);

        subtask.Title = request.Title;
        subtask.IsCompleted = request.IsCompleted;
        subtask.SortOrder = request.SortOrder;

        _uow.Subtasks.Update(subtask);
        await _uow.SaveChangesAsync(ct);
        return SubtaskDto.FromEntity(subtask);
    }
}

using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Subtasks.Commands;

public record DeleteSubtaskCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteSubtaskCommandHandler : IRequestHandler<DeleteSubtaskCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteSubtaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteSubtaskCommand request, CancellationToken ct)
    {
        var subtask = await _uow.Subtasks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Subtask", request.Id);

        _uow.Subtasks.Delete(subtask);
        await _uow.SaveChangesAsync(ct);
    }
}

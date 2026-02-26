using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Tasks.Commands;

public record DeleteTaskCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Task", request.Id);

        _uow.Tasks.Delete(task);
        await _uow.SaveChangesAsync(ct);
    }
}

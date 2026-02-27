using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ProgressEntries.Commands;

public record DeleteProgressEntryCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteProgressEntryCommandHandler : IRequestHandler<DeleteProgressEntryCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteProgressEntryCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteProgressEntryCommand request, CancellationToken ct)
    {
        var entry = await _uow.ProgressEntries.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ProgressEntry", request.Id);

        _uow.ProgressEntries.Delete(entry);
        await _uow.SaveChangesAsync(ct);
    }
}

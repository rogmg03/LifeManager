using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Clients.Commands;

public record DeleteClientCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteClientCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteClientCommand request, CancellationToken ct)
    {
        var client = await _uow.Clients.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Client", request.Id);

        // Soft delete — set inactive
        client.Status = ClientStatus.Inactive;
        _uow.Clients.Update(client);
        await _uow.SaveChangesAsync(ct);
    }
}

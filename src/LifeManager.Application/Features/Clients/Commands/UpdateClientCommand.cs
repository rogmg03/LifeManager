using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Clients.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Clients.Commands;

public record UpdateClientCommand(
    Guid Id,
    string Name,
    string? ContactPerson,
    Priority Priority,
    ClientStatus Status,
    string? Notes,
    string Color) : IRequest<ClientDto>, IBaseCommand;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateClientCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken ct)
    {
        var client = await _uow.Clients.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Client", request.Id);

        client.Name = request.Name;
        client.ContactPerson = request.ContactPerson;
        client.Priority = request.Priority;
        client.Status = request.Status;
        client.Notes = request.Notes;
        client.Color = request.Color;

        _uow.Clients.Update(client);
        await _uow.SaveChangesAsync(ct);
        return ClientDto.FromEntity(client);
    }
}

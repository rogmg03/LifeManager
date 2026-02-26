using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Clients.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Clients.Commands;

public record CreateClientCommand(
    string Name,
    string? ContactPerson,
    Priority Priority,
    string? Notes,
    string Color = "#6366F1") : IRequest<ClientDto>, IBaseCommand;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateClientCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken ct)
    {
        var client = new Client
        {
            UserId = _currentUser.UserId,
            Name = request.Name,
            ContactPerson = request.ContactPerson,
            Priority = request.Priority,
            Notes = request.Notes,
            Color = request.Color
        };
        await _uow.Clients.AddAsync(client, ct);
        await _uow.SaveChangesAsync(ct);
        return ClientDto.FromEntity(client);
    }
}

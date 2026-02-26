using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Clients.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Clients.Queries;

public record GetClientsQuery : IRequest<List<ClientDto>>;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetClientsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken ct)
    {
        var clients = await _uow.Clients.GetAllByUserIdAsync(_currentUser.UserId, ct);
        return clients.Select(ClientDto.FromEntity).ToList();
    }
}

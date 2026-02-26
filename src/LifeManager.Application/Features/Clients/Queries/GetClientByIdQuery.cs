using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Clients.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Clients.Queries;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDto>;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly IUnitOfWork _uow;
    public GetClientByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken ct)
    {
        var client = await _uow.Clients.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Client", request.Id);
        return ClientDto.FromEntity(client);
    }
}

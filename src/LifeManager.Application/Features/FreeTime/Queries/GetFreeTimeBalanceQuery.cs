using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.FreeTime.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTime.Queries;

public record GetFreeTimeBalanceQuery : IRequest<FreeTimeBalanceDto>;

public class GetFreeTimeBalanceQueryHandler : IRequestHandler<GetFreeTimeBalanceQuery, FreeTimeBalanceDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetFreeTimeBalanceQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<FreeTimeBalanceDto> Handle(GetFreeTimeBalanceQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var balanceMinutes = await _uow.FreeTimeTransactions.GetLatestBalanceForUserAsync(userId, ct);
        return new FreeTimeBalanceDto(balanceMinutes);
    }
}

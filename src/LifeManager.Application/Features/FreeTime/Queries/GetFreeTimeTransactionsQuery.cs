using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Common.Models;
using LifeManager.Application.Features.FreeTime.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTime.Queries;

public record GetFreeTimeTransactionsQuery(
    DateTime? From,
    DateTime? To,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<FreeTimeTransactionDto>>;

public class GetFreeTimeTransactionsQueryHandler
    : IRequestHandler<GetFreeTimeTransactionsQuery, PagedResult<FreeTimeTransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetFreeTimeTransactionsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<PagedResult<FreeTimeTransactionDto>> Handle(
        GetFreeTimeTransactionsQuery request, CancellationToken ct)
    {
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var page = Math.Max(request.Page, 1);

        var (items, totalCount) = await _uow.FreeTimeTransactions.GetPagedByUserIdAsync(
            _currentUser.UserId, request.From, request.To, page, pageSize, ct);

        return new PagedResult<FreeTimeTransactionDto>(
            items.Select(FreeTimeTransactionDto.FromEntity).ToList(),
            totalCount,
            page,
            pageSize);
    }
}

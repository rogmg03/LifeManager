using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Common.Models;
using LifeManager.Application.Features.Activity.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Activity.Queries;

public record GetActivityFeedQuery(
    Guid? ProjectId,
    ActivityType? Type,
    DateTime? From,
    DateTime? To,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<ActivityEntryDto>>;

public class GetActivityFeedQueryHandler
    : IRequestHandler<GetActivityFeedQuery, PagedResult<ActivityEntryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetActivityFeedQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<PagedResult<ActivityEntryDto>> Handle(
        GetActivityFeedQuery request, CancellationToken ct)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (items, total) = await _uow.ActivityEntries.GetPagedAsync(
            _currentUser.UserId,
            request.ProjectId,
            request.Type,
            request.From,
            request.To,
            page,
            pageSize,
            ct);

        return new PagedResult<ActivityEntryDto>(
            items.Select(ActivityEntryDto.FromEntity).ToList(),
            total,
            page,
            pageSize);
    }
}

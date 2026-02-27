using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.FreeTimeRatios.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTimeRatios.Queries;

public record GetFreeTimeRatioQuery : IRequest<FreeTimeRatioDto>;

public class GetFreeTimeRatioQueryHandler : IRequestHandler<GetFreeTimeRatioQuery, FreeTimeRatioDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetFreeTimeRatioQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<FreeTimeRatioDto> Handle(GetFreeTimeRatioQuery request, CancellationToken ct)
    {
        var ratio = await _uow.FreeTimeRatios.GetByUserIdAsync(_currentUser.UserId, ct);

        // Return default (1:1) if not yet configured
        if (ratio is null)
            return new FreeTimeRatioDto(Guid.Empty, 1.0m);

        return FreeTimeRatioDto.FromEntity(ratio);
    }
}

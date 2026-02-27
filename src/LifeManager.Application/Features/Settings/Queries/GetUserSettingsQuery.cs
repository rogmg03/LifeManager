using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Settings.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Settings.Queries;

public record GetUserSettingsQuery : IRequest<UserSettingsDto>;

public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetUserSettingsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<UserSettingsDto> Handle(GetUserSettingsQuery request, CancellationToken ct)
    {
        var settings = await _uow.UserSettings.GetByUserIdAsync(_currentUser.UserId, ct)
            ?? throw new NotFoundException("UserSettings", _currentUser.UserId);

        return UserSettingsDto.FromEntity(settings);
    }
}

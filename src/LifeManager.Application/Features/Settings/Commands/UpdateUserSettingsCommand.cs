using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Settings.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Settings.Commands;

public record UpdateUserSettingsCommand(
    Theme Theme,
    string TimeZone,
    int DailyWorkGoalMinutes,
    int FreeTimeRatioPercent) : IRequest<UserSettingsDto>, IBaseCommand;

public class UpdateUserSettingsCommandHandler : IRequestHandler<UpdateUserSettingsCommand, UserSettingsDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateUserSettingsCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<UserSettingsDto> Handle(UpdateUserSettingsCommand request, CancellationToken ct)
    {
        var settings = await _uow.UserSettings.GetByUserIdAsync(_currentUser.UserId, ct)
            ?? throw new NotFoundException("UserSettings", _currentUser.UserId);

        settings.Theme = request.Theme;
        settings.TimeZone = request.TimeZone;
        settings.DailyWorkGoalMinutes = request.DailyWorkGoalMinutes;
        settings.FreeTimeRatioPercent = request.FreeTimeRatioPercent;

        _uow.UserSettings.Update(settings);
        await _uow.SaveChangesAsync(ct);

        return UserSettingsDto.FromEntity(settings);
    }
}

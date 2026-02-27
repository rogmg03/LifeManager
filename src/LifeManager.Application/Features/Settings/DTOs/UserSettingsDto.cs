using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.Settings.DTOs;

public record UserSettingsDto(
    Guid Id,
    Theme Theme,
    string TimeZone,
    int DailyWorkGoalMinutes,
    int FreeTimeRatioPercent,
    DateTime UpdatedAt)
{
    public static UserSettingsDto FromEntity(UserSettings s) => new(
        s.Id, s.Theme, s.TimeZone, s.DailyWorkGoalMinutes, s.FreeTimeRatioPercent, s.UpdatedAt);
}

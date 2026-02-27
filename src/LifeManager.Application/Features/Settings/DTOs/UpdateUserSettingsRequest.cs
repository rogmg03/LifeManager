using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.Settings.DTOs;

public record UpdateUserSettingsRequest(
    Theme Theme,
    string TimeZone,
    int DailyWorkGoalMinutes,
    int FreeTimeRatioPercent);

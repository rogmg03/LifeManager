namespace LifeManager.Application.Features.FreeTime.DTOs;

public record FreeTimeDailySummaryDto(
    DateTime Date,
    int WorkedMinutes,
    int EarnedMinutes,
    int SpentMinutes,
    int BalanceAtEndOfDay);

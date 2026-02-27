using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.FreeTimeRatios.DTOs;

public record FreeTimeRatioDto(Guid Id, decimal WorkMinutesPerFreeMinute)
{
    public static FreeTimeRatioDto FromEntity(FreeTimeRatio r) => new(r.Id, r.WorkMinutesPerFreeMinute);
}

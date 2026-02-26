using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Labels.DTOs;

public record LabelDto(
    Guid Id,
    string Name,
    string Color,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static LabelDto FromEntity(Label l) => new(
        l.Id, l.Name, l.Color, l.CreatedAt, l.UpdatedAt);
}

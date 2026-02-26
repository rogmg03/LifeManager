using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Clients.DTOs;

public record ClientDto(
    Guid Id,
    string Name,
    string? ContactPerson,
    string Priority,
    string Status,
    string? Notes,
    string Color,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ClientDto FromEntity(Client c) => new(
        c.Id, c.Name, c.ContactPerson,
        c.Priority.ToString(), c.Status.ToString(),
        c.Notes, c.Color, c.CreatedAt, c.UpdatedAt);
}

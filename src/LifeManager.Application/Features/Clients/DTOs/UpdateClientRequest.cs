using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Clients.DTOs;

public record UpdateClientRequest(
    string Name,
    string? ContactPerson,
    Priority Priority,
    ClientStatus Status,
    string? Notes,
    string Color);

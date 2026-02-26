using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Clients.DTOs;

public record CreateClientRequest(
    string Name,
    string? ContactPerson,
    Priority Priority,
    string? Notes,
    string Color = "#6366F1");

namespace LifeManager.Application.Features.Auth.DTOs;

public record AuthResponseDto(
    string Token,
    Guid UserId,
    string Email,
    string Name,
    string? AvatarUrl);

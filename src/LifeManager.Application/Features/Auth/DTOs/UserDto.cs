namespace LifeManager.Application.Features.Auth.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string Name,
    string? AvatarUrl,
    string Theme,
    string TimeZone);

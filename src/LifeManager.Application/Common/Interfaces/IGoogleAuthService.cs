namespace LifeManager.Application.Common.Interfaces;

public record GoogleUserInfo(string GoogleId, string Email, string Name, string? AvatarUrl);

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> ValidateGoogleTokenAsync(string idToken, CancellationToken cancellationToken = default);
}

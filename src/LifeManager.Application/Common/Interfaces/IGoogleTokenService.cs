namespace LifeManager.Application.Common.Interfaces;

public interface IGoogleTokenService
{
    /// <summary>
    /// Exchanges a one-time authorization code for a refresh token and access token.
    /// </summary>
    Task<(string AccessToken, string RefreshToken)> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken ct = default);

    /// <summary>
    /// Uses a stored refresh token to obtain a new short-lived access token.
    /// </summary>
    Task<string> RefreshAccessTokenAsync(
        string refreshToken,
        CancellationToken ct = default);
}

using System.Net.Http.Json;
using System.Text.Json.Serialization;
using LifeManager.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LifeManager.Infrastructure.Services;

public class GoogleTokenService : IGoogleTokenService
{
    private const string TokenEndpoint = "https://oauth2.googleapis.com/token";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public GoogleTokenService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _clientId = config["Google:ClientId"]
            ?? throw new InvalidOperationException("Google:ClientId not configured.");
        _clientSecret = config["Google:ClientSecret"]
            ?? throw new InvalidOperationException("Google:ClientSecret not configured.");
    }

    public async Task<(string AccessToken, string RefreshToken)> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken ct = default)
    {
        var payload = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        };

        var response = await PostTokenAsync(payload, ct);

        if (string.IsNullOrEmpty(response.RefreshToken))
            throw new InvalidOperationException(
                "No refresh_token returned. Ensure 'access_type=offline' and 'prompt=consent' were used.");

        return (response.AccessToken, response.RefreshToken);
    }

    public async Task<string> RefreshAccessTokenAsync(
        string refreshToken,
        CancellationToken ct = default)
    {
        var payload = new Dictionary<string, string>
        {
            ["refresh_token"] = refreshToken,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["grant_type"] = "refresh_token"
        };

        var response = await PostTokenAsync(payload, ct);
        return response.AccessToken;
    }

    private async Task<GoogleTokenResponse> PostTokenAsync(
        Dictionary<string, string> payload,
        CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient();
        var httpResponse = await client.PostAsync(
            TokenEndpoint,
            new FormUrlEncodedContent(payload),
            ct);

        httpResponse.EnsureSuccessStatusCode();

        return await httpResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>(
            cancellationToken: ct)
            ?? throw new InvalidOperationException("Failed to parse Google token response.");
    }

    private sealed class GoogleTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
